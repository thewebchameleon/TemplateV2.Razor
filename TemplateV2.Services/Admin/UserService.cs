using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Common.Extensions;
using TemplateV2.Common.Notifications;
using TemplateV2.Infrastructure.Authentication;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Models;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Models.ServiceModels.Admin.Users;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Services.Admin
{
    public class UserService : IUserService
    {
        #region Instance Fields

        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;
        private readonly IEmailManager _emailManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly ICacheManager _cache;

        #endregion

        #region Constructor

        public UserService(
            IAccountService accountService,
            ISessionManager sessionManager,
            IEmailManager emailManager,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWorkFactory uowFactory,
            ICacheManager cache)
        {
            _uowFactory = uowFactory;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _accountService = accountService;
            _sessionManager = sessionManager;
            _emailManager = emailManager;
        }

        #endregion

        #region Public Methods

        public async Task<GetUsersResponse> GetUsers()
        {
            var response = new GetUsersResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                response.Users = await uow.UserRepo.GetUsers();
                uow.Commit();

                return response;
            }
        }

        public async Task<EnableUserResponse> EnableUser(EnableUserRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();

            var response = new EnableUserResponse();

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.Id
                });

                await uow.UserRepo.EnableUser(new Repositories.DatabaseRepos.UserRepo.Models.EnableUserRequest()
                {
                    Id = request.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.UserEnabled,
                    Info = new Dictionary<string, string>()
                    {
                       { "User_Id", request.Id.ToString() }
                    }
                });
            }

            response.Notifications.Add($"User '{user.Username}' has been enabled", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<DisableUserResponse> DisableUser(DisableUserRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new DisableUserResponse();

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.Id
                });

                await uow.UserRepo.DisableUser(new Repositories.DatabaseRepos.UserRepo.Models.DisableUserRequest()
                {
                    Id = request.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.UserDisabled,
                    Info = new Dictionary<string, string>()
                    {
                       { "User_Id", request.Id.ToString() }
                    }
                });
            }

            response.Notifications.Add($"User '{user.Username}' has been disabled", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            var response = new GetUserResponse();

            var userRoles = await _cache.UserRoles();
            var roles = await _cache.Roles();
            var permissions = await _cache.Permissions();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.Id
                });
                uow.Commit();

                var usersRoles = userRoles.Where(ur => ur.User_Id == request.Id).Select(ur => ur.Role_Id);

                response.Roles = roles.Where(r => usersRoles.Contains(r.Id)).ToList();

                if (user == null)
                {
                    response.Notifications.AddError($"Could not find user with Id {request.Id}");
                    return response;
                }
                response.User = user;
                return response;
            }
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreateUserResponse();
            var username = request.EmailAddress;

            var duplicateResponse = await _accountService.DuplicateUserCheck(new DuplicateUserCheckRequest()
            {
                EmailAddress = request.EmailAddress,
                Username = username
            });

            if (duplicateResponse.Notifications.HasErrors)
            {
                response.Notifications.Add(duplicateResponse.Notifications);
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.UserRepo.CreateUser(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserRequest()
                {
                    Username = username,
                    First_Name = request.FirstName,
                    Last_Name = request.LastName,
                    Mobile_Number = request.MobileNumber,
                    Email_Address = request.EmailAddress,
                    Password_Hash = PasswordHelper.HashPassword(request.Password),
                    Created_By = sessionUser.Id,
                    Is_Enabled = true
                });
                uow.Commit();
            }

            await CreateOrDeleteUserRoles(request.RoleIds, id, sessionUser.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.UserCreated,
                Info = new Dictionary<string, string>()
                {
                    { "User_Id", id.ToString() }
                }
            });

            response.Notifications.Add($"User {request.Username} has been created", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdateUserResponse();

            var duplicateResponse = await _accountService.DuplicateUserCheck(new DuplicateUserCheckRequest()
            {
                EmailAddress = request.EmailAddress,
                Username = request.Username,
                MobileNumber = request.MobileNumber,
                UserId = request.Id
            });

            if (duplicateResponse.Notifications.HasErrors)
            {
                response.Notifications.Add(duplicateResponse.Notifications);
                return response;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.Id
                });

                var dbRequest = new Repositories.DatabaseRepos.UserRepo.Models.UpdateUserRequest()
                {
                    Id = request.Id,
                    Username = request.Username,
                    First_Name = request.FirstName,
                    Last_Name = request.LastName,
                    Mobile_Number = request.MobileNumber,
                    Email_Address = request.EmailAddress,
                    Updated_By = sessionUser.Id,
                };

                await uow.UserRepo.UpdateUser(dbRequest);
                uow.Commit();
            }

            await CreateOrDeleteUserRoles(request.RoleIds, request.Id, sessionUser.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.UserUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "User_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"User {request.Username} has been updated", NotificationTypeEnum.Success);
            return response;
        }

        private async Task CreateOrDeleteUserRoles(List<int> newRoles, int userId, int loggedInUserId)
        {
            var userRoles = await _cache.UserRoles();
            var existingRoles = userRoles.Where(ur => ur.User_Id == userId).Select(ur => ur.Role_Id);

            var rolesToBeDeleted = existingRoles.Where(ur => !newRoles.Contains(ur));
            var rolesToBeCreated = newRoles.Where(ur => !existingRoles.Contains(ur));

            if (rolesToBeDeleted.Any() || rolesToBeCreated.Any())
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    foreach (var roleId in rolesToBeCreated)
                    {
                        await uow.UserRepo.CreateUserRole(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserRoleRequest()
                        {
                            User_Id = userId,
                            Role_Id = roleId,
                            Created_By = loggedInUserId
                        });

                    }

                    foreach (var roleId in rolesToBeDeleted)
                    {
                        await uow.UserRepo.DeleteUserRole(new Repositories.DatabaseRepos.UserRepo.Models.DeleteUserRoleRequest()
                        {
                            User_Id = userId,
                            Role_Id = roleId,
                            Updated_By = loggedInUserId
                        });

                    }
                    uow.Commit();
                }
                _cache.Remove(CacheConstants.UserRoles);

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.UserRolesUpdated,
                    Info = new Dictionary<string, string>()
                    {
                       { "User_Id", userId.ToString() }
                    }
                });
            }
        }

        public async Task<UnlockUserResponse> UnlockUser(UnlockUserRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UnlockUserResponse();

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.Id
                });

                await uow.UserRepo.UnlockUser(new Repositories.DatabaseRepos.UserRepo.Models.UnlockUserRequest()
                {
                    Id = request.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.UserUnlocked,
                    Info = new Dictionary<string, string>()
                    {
                       { "User_Id", request.Id.ToString() }
                    }
                });
            }

            response.Notifications.Add($"User '{user.Username}' has been unlocked", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<ConfirmRegistrationResponse> ConfirmRegistration(ConfirmRegistrationRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new ConfirmRegistrationResponse();

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.Id
                });

                await uow.UserRepo.ActivateAccount(new Repositories.DatabaseRepos.UserRepo.Models.ActivateAccountRequest()
                {
                    Id = request.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.UserActivated,
                    Info = new Dictionary<string, string>()
                    {
                       { "User_Id", request.Id.ToString() }
                    }
                });
            }

            response.Notifications.Add($"User '{user.Username}' has been activated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<GenerateResetPasswordUrlResponse> GenerateResetPasswordUrl(GenerateResetPasswordUrlRequest request)
        {
            var response = new GenerateResetPasswordUrlResponse();

            var resetToken = string.Empty;

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.UserId
                });

                resetToken = GenerateUniqueUserToken(uow);

                await uow.UserRepo.CreateUserToken(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserTokenRequest()
                {
                    User_Id = request.UserId,
                    Token = new Guid(resetToken),
                    Type_Id = (int)TokenTypeEnum.ResetPassword,
                    Created_By = ApplicationConstants.SystemUserId,
                });
                uow.Commit();
            }

            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            response.Url = $"{baseUrl}/Account/ResetPassword?token={resetToken}";

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.ResetPasswordUrlGenerated,
                Info = new Dictionary<string, string>()
                {
                    { "User_Id", request.UserId.ToString() }
                }
            });

            return response;
        }

        public async Task<SendResetPasswordEmailResponse> SendResetPasswordEmail(SendResetPasswordEmailRequest request)
        {
            var response = new SendResetPasswordEmailResponse();

            await _emailManager.SendResetPassword(new Models.ServiceModels.Email.SendResetPasswordRequest()
            {
                UserId = request.UserId
            });

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.ResetPasswordEmailSent,
                Info = new Dictionary<string, string>()
                    {
                       { "User_Id", request.UserId.ToString() }
                    }
            });

            return response;
        }

        #endregion

        #region Private Methods

        private string GenerateUniqueUserToken(IUnitOfWork uow)
        {
            var generatedCode = GenerateGuid();

            while (CheckUserTokenExists(uow, generatedCode))
            {
                generatedCode = GenerateGuid();
            }
            return generatedCode;
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        private bool CheckUserTokenExists(IUnitOfWork uow, string token)
        {
            var tokenResult = uow.UserRepo.GetUserTokenByGuid(new Repositories.DatabaseRepos.UserRepo.Models.GetUserTokenByGuidRequest()
            {
                Guid = new Guid(token)
            });
            tokenResult.Wait();
            return tokenResult.Result != null;
        }

        #endregion
    }
}
