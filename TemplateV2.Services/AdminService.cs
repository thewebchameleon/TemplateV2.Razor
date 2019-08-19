using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Common.Extensions;
using TemplateV2.Common.Notifications;
using TemplateV2.Infrastructure.Authentication;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Models;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Models.ServiceModels.Admin.Configuration;
using TemplateV2.Models.ServiceModels.Admin.Permissions;
using TemplateV2.Models.ServiceModels.Admin.Roles;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Models.ServiceModels.Admin.Sessions;
using TemplateV2.Models.ServiceModels.Admin.Users;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;
using Newtonsoft.Json;

namespace TemplateV2.Services
{
    public class AdminService : IAdminService
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

        public AdminService(
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

        #region User Management

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

        #region Role Management

        public async Task<GetRolesResponse> GetRoles()
        {
            var response = new GetRolesResponse
            {
                Roles = await _cache.Roles()
            };

            return response;
        }

        public async Task<EnableRoleResponse> EnableRole(EnableRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new EnableRoleResponse();

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(u => u.Id == request.Id);

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.EnableRole(new Repositories.DatabaseRepos.UserRepo.Models.EnableRoleRequest()
                {
                    Id = role.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Roles);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleEnabled,
                Info = new Dictionary<string, string>()
                {
                    { "Role_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"Role '{role.Name}' has been enabled", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<DisableRoleResponse> DisableRole(DisableRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new DisableRoleResponse();

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(u => u.Id == request.Id);

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.DisableRole(new Repositories.DatabaseRepos.UserRepo.Models.DisableRoleRequest()
                {
                    Id = role.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Roles);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleDisabled,
                Info = new Dictionary<string, string>()
                {
                    { "Role_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"Role '{role.Name}' has been disabled", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<GetRoleResponse> GetRole(GetRoleRequest request)
        {
            var response = new GetRoleResponse();

            var roles = await _cache.Roles();
            var permissions = await _cache.Permissions();
            var rolePermissions = await _cache.RolePermissions();

            var role = roles.FirstOrDefault(r => r.Id == request.Id);
            if (role == null)
            {
                response.Notifications.AddError($"Could not find role with Id {request.Id}");
                return response;
            }

            var rolesPermissions = rolePermissions.Where(rc => rc.Role_Id == request.Id).Select(rc => rc.Permission_Id);

            response.Role = role;
            response.Permissions = permissions.Where(c => rolesPermissions.Contains(c.Id)).ToList();

            response.Role = role;
            return response;
        }

        public async Task<CreateRoleResponse> CreateRole(CreateRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreateRoleResponse();

            var duplicateResponse = await _accountService.DuplicateRoleCheck(new DuplicateRoleCheckRequest()
            {
                Name = request.Name
            });

            if (duplicateResponse.Notifications.HasErrors)
            {
                response.Notifications.Add(duplicateResponse.Notifications);
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.UserRepo.CreateRole(new Repositories.DatabaseRepos.UserRepo.Models.CreateRoleRequest()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Created_By = sessionUser.Id,
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Roles);

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(r => r.Id == id);

            await CreateOrDeleteRolePermissions(request.PermissionIds, id, sessionUser.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleCreated,
                Info = new Dictionary<string, string>()
                {
                    { "Role Name", request.Name.ToString() }
                }
            });

            response.Notifications.Add($"Role '{request.Name}' has been created", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<UpdateRoleResponse> UpdateRole(UpdateRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdateRoleResponse();

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(u => u.Id == request.Id);

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.UpdateRole(new Repositories.DatabaseRepos.UserRepo.Models.UpdateRoleRequest()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            await CreateOrDeleteRolePermissions(request.PermissionIds, request.Id, sessionUser.Id);

            _cache.Remove(CacheConstants.Roles);
            _cache.Remove(CacheConstants.RolePermissions);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "Role_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"Role '{role.Name}' has been updated", NotificationTypeEnum.Success);
            return response;
        }

        private async Task CreateOrDeleteRolePermissions(List<int> newPermissions, int roleId, int loggedInUserId)
        {
            var rolePermissions = await _cache.RolePermissions();
            var existingPermissions = rolePermissions.Where(rc => rc.Role_Id == roleId).Select(rc => rc.Permission_Id);

            var permissionsToBeDeleted = existingPermissions.Where(ur => !newPermissions.Contains(ur));
            var permissionsToBeCreated = newPermissions.Where(ur => !existingPermissions.Contains(ur));

            if (permissionsToBeDeleted.Any() || permissionsToBeCreated.Any())
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    foreach (var permissionId in permissionsToBeCreated)
                    {
                        await uow.UserRepo.CreateRolePermission(new Repositories.DatabaseRepos.UserRepo.Models.CreateRolePermissionRequest()
                        {
                            Role_Id = roleId,
                            Permission_Id = permissionId,
                            Created_By = loggedInUserId
                        });

                    }

                    foreach (var permissionId in permissionsToBeDeleted)
                    {
                        await uow.UserRepo.DeleteRolePermission(new Repositories.DatabaseRepos.UserRepo.Models.DisableRolePermissionRequest()
                        {
                            Role_Id = roleId,
                            Permission_Id = permissionId,
                            Updated_By = loggedInUserId
                        });

                    }
                    uow.Commit();
                }
                _cache.Remove(CacheConstants.RolePermissions);

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.RolePermissionsUpdated,
                    Info = new Dictionary<string, string>()
                    {
                       { "Role_Id", roleId.ToString() }
                    }
                });
            }
        }

        #endregion

        #region Configuration Management

        public async Task<GetConfigurationItemsResponse> GetConfigurationItems()
        {
            var response = new GetConfigurationItemsResponse();

            var configuration = await _cache.Configuration();
            response.ConfigurationItems = configuration.Items;

            return response;
        }

        public async Task<GetConfigurationItemResponse> GetConfigurationItem(GetConfigurationItemRequest request)
        {
            var response = new GetConfigurationItemResponse();

            var configuration = await _cache.Configuration();
            var configItem = configuration.Items.FirstOrDefault(c => c.Id == request.Id);

            response.ConfigurationItem = configItem;

            return response;
        }

        public async Task<UpdateConfigurationItemResponse> UpdateConfigurationItem(UpdateConfigurationItemRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdateConfigurationItemResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.ConfigurationRepo.UpdateConfigurationItem(new Repositories.DatabaseRepos.ConfigurationRepo.Models.UpdateConfigurationItemRequest()
                {
                    Id = request.Id,
                    Description = request.Description,
                    Boolean_Value = request.BooleanValue,
                    DateTime_Value = request.DateTimeValue,
                    Date_Value = request.DateValue,
                    Time_Value = request.TimeValue,
                    Decimal_Value = request.DecimalValue,
                    Int_Value = request.IntValue,
                    Money_Value = request.MoneyValue,
                    String_Value = request.StringValue,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.ConfigurationItems);

            var configuration = await _cache.Configuration();
            var configItem = configuration.Items.FirstOrDefault(c => c.Id == request.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.ConfigurationUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", configItem.Key.ToString() }
                }
            });

            response.Notifications.Add($"Configuration item '{configItem.Key}' has been updated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<CreateConfigurationItemResponse> CreateConfigurationItem(CreateConfigurationItemRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreateConfigurationItemResponse();

            var configuration = await _cache.Configuration();
            var configItem = configuration.Items.FirstOrDefault(c => c.Key == request.Key);

            if (configItem != null)
            {
                response.Notifications.AddError($"A configuration item already exists with the key {request.Key}");
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.ConfigurationRepo.CreateConfigurationItem(new Repositories.DatabaseRepos.ConfigurationRepo.Models.CreateConfigurationItemRequest()
                {
                    Key = request.Key,
                    Description = request.Description,
                    Boolean_Value = request.BooleanValue,
                    DateTime_Value = request.DateTimeValue,
                    Date_Value = request.DateValue,
                    Time_Value = request.TimeValue,
                    Decimal_Value = request.DecimalValue,
                    Int_Value = request.IntValue,
                    Money_Value = request.MoneyValue,
                    String_Value = request.StringValue,
                    Created_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.ConfigurationItems);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.ConfigurationCreated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", request.Key.ToString() }
                }
            });

            response.Notifications.Add($"Configuration item '{request.Key}' has been created", NotificationTypeEnum.Success);
            return response;
        }

        #endregion

        #region Sessions

        public async Task<GetSessionResponse> GetSession(GetSessionRequest request)
        {
            var response = new GetSessionResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var session = await uow.SessionRepo.GetSessionById(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionByIdRequest()
                {
                    Id = request.Id
                });

                if (session == null)
                {
                    response.Notifications.AddError($"Could not find session with Id {request.Id}");
                    return response;
                }

                if (session.User_Id.HasValue)
                {
                    var user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                    {
                        Id = session.User_Id.Value
                    });
                    response.User = user;
                }

                response.Session = session;

                uow.Commit();
                return response;
            }
        }

        public async Task<GetSessionLogsResponse> GetSessionLogs(GetSessionLogsRequest request)
        {
            var response = new GetSessionLogsResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var logs = await uow.SessionRepo.GetSessionLogsBySessionId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionLogsBySessionIdRequest()
                {
                    Session_Id = request.Session_Id
                });
                var logEvents = await uow.SessionRepo.GetSessionLogEventsBySessionId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionLogEventsBySessionIdRequest()
                {
                    Session_Id = request.Session_Id
                });

                var eventsLookup = await _cache.SessionEvents();
                response.Logs = logs.Select(l =>
                {
                    var eventIds = logEvents.Where(le => le.Session_Log_Id == l.Id).Select(le => le.Event_Id);
                    return new SessionLog()
                    {
                        Entity = l,
                        Events = eventsLookup.Where(e => eventIds.Contains(e.Id)).Select(e =>
                        {
                            var infoDictionary = new Dictionary<string, string>();
                            var infoRaw = logEvents.FirstOrDefault(le => le.Event_Id == e.Id).InfoDictionary_JSON;
                            if (!string.IsNullOrEmpty(infoRaw))
                            {
                                infoDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(infoRaw);
                            }

                            return new SessionLogEvent()
                            {
                                Event = e,
                                Info = infoDictionary,
                                Event_Date = logEvents.FirstOrDefault(le => le.Event_Id == e.Id).Created_Date
                            };
                        }).ToList()
                    };

                }).ToList();

                uow.Commit();
                return response;
            }



        }

        public async Task<GetSessionsResponse> GetSessions(GetSessionsRequest request)
        {
            var response = new GetSessionsResponse();
            var sessions = new List<Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsResponse>();

            if (request.Last24Hours.HasValue && request.Last24Hours.Value)
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    sessions = await uow.SessionRepo.GetAdminSessionsByStartDate(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByStartDateRequest()
                    {
                        Start_Date = DateTime.Now.AddDays(-1)
                    });

                    uow.Commit();
                }
                response.SelectedFilter = "Last 24 Hours";
            }

            if (request.LastXDays.HasValue)
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    sessions = await uow.SessionRepo.GetAdminSessionsByStartDate(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByStartDateRequest()
                    {
                        Start_Date = DateTime.Today.AddDays(request.LastXDays.Value * -1)
                    });

                    uow.Commit();
                }
                response.SelectedFilter = $"Last {request.LastXDays} days";
            }

            if (request.Day.HasValue)
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    sessions = await uow.SessionRepo.GetAdminSessionsByDate(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByDateRequest()
                    {
                        Date = request.Day.Value
                    });

                    uow.Commit();
                }
                response.SelectedFilter = request.Day.Value.ToLongDateString();
            }

            if (request.UserId.HasValue)
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    sessions = await uow.SessionRepo.GetAdminSessionsByUserId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByUserIdRequest()
                    {
                        User_Id = request.UserId.Value
                    });

                    uow.Commit();
                }
                response.SelectedFilter = $"User ID: {request.UserId}";
            }

            if (!string.IsNullOrEmpty(request.Username))
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    var user = await uow.UserRepo.GetUserByUsername(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByUsernameRequest()
                    {
                        Username = request.Username
                    });

                    if (user == null)
                    {
                        response.Notifications.AddError($"Could not find user with username {request.Username}");
                        return response;
                    }

                    sessions = await uow.SessionRepo.GetAdminSessionsByUserId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByUserIdRequest()
                    {
                        User_Id = user.Id
                    });

                    uow.Commit();
                }
                response.SelectedFilter = $"Username: {request.Username}";
            }

            if (!string.IsNullOrEmpty(request.MobileNumber))
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    var user = await uow.UserRepo.GetUserByMobileNumber(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByMobileNumberRequest()
                    {
                        Mobile_Number = request.MobileNumber
                    });

                    if (user == null)
                    {
                        response.Notifications.AddError($"Could not find user with mobile number {request.MobileNumber}");
                        return response;
                    }

                    sessions = await uow.SessionRepo.GetAdminSessionsByUserId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByUserIdRequest()
                    {
                        User_Id = user.Id
                    });

                    uow.Commit();
                }
                response.SelectedFilter = $"Mobile Number: {request.MobileNumber}";
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    var user = await uow.UserRepo.GetUserByEmail(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByEmailRequest()
                    {
                        Email_Address = request.EmailAddress
                    });

                    if (user == null)
                    {
                        response.Notifications.AddError($"Could not find user with email address {request.EmailAddress}");
                        return response;
                    }

                    sessions = await uow.SessionRepo.GetAdminSessionsByUserId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionsByUserIdRequest()
                    {
                        User_Id = user.Id
                    });

                    uow.Commit();
                }
                response.SelectedFilter = $"Email Address: {request.EmailAddress}";
            }

            response.Sessions = sessions.Select(s => new Session()
            {
                Entity = s,
                Username = s.Username,
                Last_Session_Event_Date = s.Last_Session_Event_Date,
                Last_Session_Log_Date = s.Last_Session_Log_Date
            }).OrderByDescending(s => s.Entity.Created_Date).ToList();
            return response;
        }

        public async Task<GetSessionEventsResponse> GetSessionEvents()
        {
            var response = new GetSessionEventsResponse
            {
                SessionEvents = await _cache.SessionEvents()
            };

            return response;
        }

        public async Task<GetSessionEventResponse> GetSessionEvent(GetSessionEventRequest request)
        {
            var response = new GetSessionEventResponse();

            var sessionEvents = await _cache.SessionEvents();
            var sessionEvent = sessionEvents.FirstOrDefault(c => c.Id == request.Id);

            response.SessionEvent = sessionEvent;

            return response;
        }

        public async Task<UpdateSessionEventResponse> UpdateSessionEvent(UpdateSessionEventRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdateSessionEventResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.SessionRepo.UpdateSessionEvent(new Repositories.DatabaseRepos.SessionRepo.Models.UpdateSessionEventRequest()
                {
                    Id = request.Id,
                    Description = request.Description,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.SessionEvents);

            var sessionEvents = await _cache.SessionEvents();
            var sessionEvent = sessionEvents.FirstOrDefault(c => c.Id == request.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.SessionEventUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", sessionEvent.Key.ToString() }
                }
            });

            response.Notifications.Add($"Session event '{sessionEvent.Key}' has been updated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<CreateSessionEventResponse> CreateSessionEvent(CreateSessionEventRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreateSessionEventResponse();

            var sessionEvents = await _cache.SessionEvents();
            var sessionEvent = sessionEvents.FirstOrDefault(se => se.Key == request.Key);

            if (sessionEvent != null)
            {
                response.Notifications.AddError($"A session event already exists with the key {request.Key}");
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.SessionRepo.CreateSessionEvent(new Repositories.DatabaseRepos.SessionRepo.Models.CreateSessionEventRequest()
                {
                    Key = request.Key,
                    Description = request.Description,
                    Created_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.SessionEvents);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.SessionEventCreated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", request.Key }
                }
            });

            response.Notifications.Add($"Session event '{request.Key}' has been created", NotificationTypeEnum.Success);
            return response;
        }

        #endregion

        #region Permission Management 

        public async Task<GetPermissionsResponse> GetPermissions()
        {
            var response = new GetPermissionsResponse
            {
                Permissions = await _cache.Permissions()
            };

            return response;
        }

        public async Task<GetPermissionResponse> GetPermission(GetPermissionRequest request)
        {
            var response = new GetPermissionResponse();

            var permissions = await _cache.Permissions();
            var permission = permissions.FirstOrDefault(c => c.Id == request.Id);

            response.Permission = permission;

            return response;
        }

        public async Task<UpdatePermissionResponse> UpdatePermission(UpdatePermissionRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdatePermissionResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.UpdatePermission(new Repositories.DatabaseRepos.UserRepo.Models.UpdatePermissionRequest()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Group_Name = request.GroupName,
                    Description = request.Description,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Permissions);

            var permissions = await _cache.Permissions();
            var permission = permissions.FirstOrDefault(c => c.Id == request.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.PermissionUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", permission.Key }
                }
            });

            response.Notifications.Add($"Permission '{permission.Key}' has been updated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<CreatePermissionResponse> CreatePermission(CreatePermissionRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreatePermissionResponse();

            var permissions = await _cache.Permissions();
            var permission = permissions.FirstOrDefault(c => c.Key == request.Key);

            if (permission != null)
            {
                response.Notifications.AddError($"A permission already exists with the key {request.Key}");
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.UserRepo.CreatePermission(new Repositories.DatabaseRepos.UserRepo.Models.CreatePermissionRequest()
                {
                    Key = request.Key,
                    Description = request.Description,
                    Group_Name = request.GroupName,
                    Name = request.Name,
                    Created_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Permissions);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.PermissionCreated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", request.Key }
                }
            });

            response.Notifications.Add($"Permission '{request.Key}' has been created", NotificationTypeEnum.Success);
            return response;
        }

        #endregion

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
