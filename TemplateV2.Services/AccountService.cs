using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Common.Extensions;
using TemplateV2.Common.Notifications;
using TemplateV2.Infrastructure.Authentication;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Infrastructure.Session.Contracts;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Models;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Models.ServiceModels.Account;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace TemplateV2.Services
{
    public class AccountService : IAccountService
    {
        #region Instance Fields

        private readonly ILogger<AccountService> _logger;

        private readonly ISessionManager _sessionManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IEmailManager _emailManager;
        private readonly ICacheManager _cache;

        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly ISessionProvider _sessionProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public AccountService(
            ILogger<AccountService> logger,
            ISessionManager sessionManager,
            IEmailManager emailManager,
            IUnitOfWorkFactory uowFactory,
            ICacheManager cache,
            ISessionProvider sessionProvider,
            IAuthenticationManager authenticationManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;

            _sessionProvider = sessionProvider;
            _uowFactory = uowFactory;

            _cache = cache;
            _sessionManager = sessionManager;
            _emailManager = emailManager;
            _authenticationManager = authenticationManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Public Methods

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var response = new RegisterResponse();
            var username = request.EmailAddress;

            var duplicateResponse = await DuplicateUserCheck(new DuplicateUserCheckRequest()
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
                    Email_Address = request.EmailAddress,
                    Password_Hash = PasswordHelper.HashPassword(request.Password),
                    Is_Enabled = true,
                    Created_By = ApplicationConstants.SystemUserId,
                });

                uow.Commit();
            }

            _cache.Remove(CacheConstants.UserRoles);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.UserRegistered,
                Info = new Dictionary<string, string>()
                {
                    { "Username", username }
                }
            });

            await _emailManager.SendAccountActivation(new Models.ServiceModels.Email.SendAccountActivationRequest()
            {
                UserId = id
            });

            response.Notifications.Add($"You have been successfully registered, please check {request.EmailAddress} for a link to activate your account.", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<ActivateAccountResponse> ActivateAccount(ActivateAccountRequest request)
        {
            var response = new ActivateAccountResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var userToken = await uow.UserRepo.GetUserTokenByGuid(new Repositories.DatabaseRepos.UserRepo.Models.GetUserTokenByGuidRequest()
                {
                    Guid = new Guid(request.Token)
                });

                if (userToken == null)
                {
                    response.Notifications.AddError("Token does not exist");
                    return response;
                }

                if (userToken.Type_Id != TokenTypeEnum.AccountActivation)
                {
                    response.Notifications.AddError("Invalid token provided");
                    return response;
                }

                if (userToken.Processed)
                {
                    response.Notifications.AddError($"This account has already been activated");
                    return response;
                }

                await uow.UserRepo.ActivateAccount(new Repositories.DatabaseRepos.UserRepo.Models.ActivateAccountRequest()
                {
                    Id = userToken.User_Id,
                    Updated_By = ApplicationConstants.SystemUserId
                });
                uow.Commit();
            }

            response.Notifications.Add($"Your account has been activated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var userToken = await uow.UserRepo.GetUserTokenByGuid(new Repositories.DatabaseRepos.UserRepo.Models.GetUserTokenByGuidRequest()
                {
                    Guid = new Guid(request.Token)
                });

                if (userToken == null)
                {
                    response.Notifications.AddError("Token does not exist");
                    return response;
                }

                if (userToken.Type_Id != TokenTypeEnum.ResetPassword)
                {
                    response.Notifications.AddError("Invalid token provided");
                    return response;
                }

                if (userToken.Processed)
                {
                    response.Notifications.AddError($"This account has already performed a password reset");
                    return response;
                }

                await uow.UserRepo.UpdateUserPassword(new Repositories.DatabaseRepos.UserRepo.Models.UpdateUserPasswordRequest()
                {
                    User_Id = userToken.User_Id,
                    Password_Hash = PasswordHelper.HashPassword(request.NewPassword),
                    Updated_By = ApplicationConstants.SystemUserId
                });

                await uow.UserRepo.ProcessUserToken(new Repositories.DatabaseRepos.UserRepo.Models.ProcessUserTokenRequest()
                {
                    Guid = userToken.Guid,
                    Updated_By = ApplicationConstants.SystemUserId
                });

                var user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = userToken.User_Id
                });

                if (!user.Registration_Confirmed)
                {
                    await uow.UserRepo.ActivateAccount(new Repositories.DatabaseRepos.UserRepo.Models.ActivateAccountRequest()
                    {
                        Id = user.Id,
                        Updated_By = ApplicationConstants.SystemUserId
                    });
                }

                uow.Commit();
            }

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.PasswordUpdated
            });

            response.Notifications.Add($"Your password has been reset.", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = new LoginResponse();
            var config = await _cache.Configuration();
            var session = await _sessionManager.GetSession();

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserByUsername(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByUsernameRequest()
                {
                    Username = request.Username
                });

                // check if we found a user or if their password was incorrect
                if (user == null || !PasswordHelper.Verify(request.Password, user.Password_Hash))
                {
                    // if we found the user, handle invalid login attempts else generic fail message
                    if (user != null)
                    {
                        // check if we need to lock the user
                        if (user.Invalid_Login_Attempts >= config.Max_Login_Attempts && !user.Is_Locked_Out)
                        {
                            await uow.UserRepo.LockoutUser(new Repositories.DatabaseRepos.UserRepo.Models.LockoutUserRequest()
                            {
                                Id = user.Id,
                                Lockout_End = DateTime.Now.AddMinutes(config.Account_Lockout_Expiry_Minutes),
                                Updated_By = ApplicationConstants.SystemUserId
                            });

                            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                            {
                                EventKey = SessionEventKeys.UserLocked
                            });
                        }
                        else
                        {
                            var dbRequest = new Repositories.DatabaseRepos.UserRepo.Models.AddInvalidLoginAttemptRequest()
                            {
                                User_Id = user.Id,
                                Updated_By = ApplicationConstants.SystemUserId
                            };

                            // if we are already locked out then extend the lockout time
                            if (user.Lockout_End.HasValue)
                            {
                                dbRequest.Lockout_End = DateTime.Now.AddMinutes(config.Account_Lockout_Expiry_Minutes);
                            }

                            await uow.UserRepo.AddInvalidLoginAttempt(dbRequest);

                        }
                        uow.Commit();
                    }
                    response.Notifications.AddError("Username or password is incorrect");
                    return response;
                }

                if (user.Is_Locked_Out)
                {
                    if (user.Lockout_End <= DateTime.Now)
                    {
                        await uow.UserRepo.UnlockUser(new Repositories.DatabaseRepos.UserRepo.Models.UnlockUserRequest()
                        {
                            Id = user.Id,
                            Updated_By = ApplicationConstants.SystemUserId
                        });
                        uow.Commit();

                        await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                        {
                            EventKey = SessionEventKeys.UserUnlocked
                        });
                    }
                    else
                    {
                        response.Notifications.AddError($"Your account has been locked, please try again in {config.Account_Lockout_Expiry_Minutes} minute(s)");
                        return response;
                    }
                }
                else if (user.Invalid_Login_Attempts > 0) // cleanup of old invalid login attempts
                {
                    await uow.UserRepo.UnlockUser(new Repositories.DatabaseRepos.UserRepo.Models.UnlockUserRequest()
                    {
                        Id = user.Id,
                        Updated_By = ApplicationConstants.SystemUserId
                    });
                    uow.Commit();
                }

                if (!user.Is_Enabled)
                {
                    response.Notifications.AddError("Your account has been disabled, please contact the website administrator");
                    return response;
                }

                if (!user.Registration_Confirmed)
                {
                    response.Notifications.AddError("Please check your email to activate your account");
                    return response;
                }

                var sessionEntity = await uow.SessionRepo.AddUserToSession(new Repositories.DatabaseRepos.SessionRepo.Models.AddUserToSessionRequest()
                {
                    Id = session.SessionEntity.Id,
                    User_Id = user.Id,
                    Updated_By = ApplicationConstants.SystemUserId
                });
                uow.Commit();
                await _sessionProvider.Set(SessionConstants.SessionEntity, sessionEntity);
            }

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.UserLoggedIn
            });

            await _authenticationManager.SignIn(session.SessionEntity.Id);

            return response;
        }

        public async Task Logout()
        {
            await _authenticationManager.SignOut();
        }

        public async Task<GetProfileResponse> GetProfile()
        {
            var user = await _sessionManager.GetUser();

            var roles = await _cache.Roles();
            var userRoles = await _cache.UserRoles();
            var usersRoles = userRoles.Where(ur => ur.User_Id == user.Id).Select(ur => ur.Role_Id);

            return new GetProfileResponse()
            {
                User = user,
                Roles = roles.Where(r => usersRoles.Contains(r.Id)).ToList()
            };
        }

        public async Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest request)
        {
            var response = new UpdateProfileResponse();
            var user = await _sessionManager.GetUser();

            var dbRequest = new Repositories.DatabaseRepos.UserRepo.Models.UpdateUserRequest()
            {
                Id = user.Id,
                Username = request.Username,
                Email_Address = request.EmailAddress,
                First_Name = request.FirstName,
                Last_Name = request.LastName,
                Mobile_Number = request.MobileNumber,
                Updated_By = user.Id
            };

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.UpdateUser(dbRequest);
                uow.Commit();
            }

            await _sessionManager.DehydrateSession(); // user has changed
            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.UserUpdatedProfile,
            });

            response.Notifications.Add("Profile updated successfully", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<DuplicateUserCheckResponse> DuplicateUserCheck(DuplicateUserCheckRequest request)
        {
            var response = new DuplicateUserCheckResponse();
            var emailAddress = request.EmailAddress.SafeTrim();
            var mobileNumber = request.MobileNumber.SafeTrim();
            var username = request.EmailAddress.SafeTrim();

            var duplicateUserRequest = new Repositories.DatabaseRepos.UserRepo.Models.FetchDuplicateUserRequest()
            {
                Username = username,
                Email_Address = emailAddress,
                Mobile_Number = mobileNumber,
            };

            // this is for when the user wants to change their email / username or mobile number
            if (request.UserId != null)
            {
                duplicateUserRequest.User_Id = request.UserId.Value;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var user = await uow.UserRepo.FetchDuplicateUser(duplicateUserRequest);
                uow.Commit();

                if (user == null)
                {
                    // no duplicate found
                    return new DuplicateUserCheckResponse();
                }

                bool matchFound = false;
                if (string.Equals(user.Email_Address, emailAddress, StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Notifications.AddError("A user is already registered with this email address");
                    matchFound = true;
                }

                if (string.Equals(user.Mobile_Number, mobileNumber, StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Notifications.AddError("A user is already registered with this mobile number");
                    matchFound = true;
                }

                if (string.Equals(user.Username, username, StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Notifications.AddError("A user is already registered with this username");
                    matchFound = true;
                }

                if (user.Is_Deleted || !user.Is_Enabled)
                {
                    response.Notifications.AddError("This user has been removed / disabled");
                    matchFound = true;
                }

                if (matchFound)
                {
                    return response;
                }
                _logger.LogError("Duplicate user found but could not determine why", $"UserId: {user.Id}", duplicateUserRequest);
                response.Notifications.AddError("An error ocurred while performing a duplicate check");
                return response;
            }
        }

        public async Task<DuplicateRoleCheckResponse> DuplicateRoleCheck(DuplicateRoleCheckRequest request)
        {
            var response = new DuplicateRoleCheckResponse();

            var roles = await _cache.Roles();
            var matchFound = roles.Any(u => string.Equals(u.Name, request.Name, StringComparison.InvariantCultureIgnoreCase));

            if (matchFound)
            {
                response.Notifications.AddError($"There is already a role with the name {request.Name}");
                return response;
            }
            return response;
        }

        public async Task<ValidateResetPasswordTokenResponse> ValidateResetPasswordToken(ValidateResetPasswordTokenRequest request)
        {
            var response = new ValidateResetPasswordTokenResponse();

            UserTokenEntity userToken;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                userToken = await uow.UserRepo.GetUserTokenByGuid(new Repositories.DatabaseRepos.UserRepo.Models.GetUserTokenByGuidRequest()
                {
                    Guid = new Guid(request.Token)
                });
                uow.Commit();
            }

            if (userToken == null)
            {
                response.Notifications.AddError("Token does not exist");
                return response;
            }

            if (userToken.Type_Id != TokenTypeEnum.ResetPassword)
            {
                response.Notifications.AddError("Invalid token provided");
                return response;
            }

            if (userToken.Processed)
            {
                response.Notifications.AddError($"This account has already performed a password reset");
                return response;
            }

            return response;
        }

        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            var response = new ForgotPasswordResponse();

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserByEmail(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByEmailRequest()
                {
                    Email_Address = request.EmailAddress
                });
                uow.Commit();
            }

            if (user != null)
            {
                await _emailManager.SendResetPassword(new Models.ServiceModels.Email.SendResetPasswordRequest()
                {
                    UserId = user.Id
                });
            }

            response.Notifications.Add("We have sent a reset password link to your email address if it matches our records", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request)
        {
            var response = new UpdatePasswordResponse();
            var user = await _sessionManager.GetUser();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.UpdateUserPassword(new Repositories.DatabaseRepos.UserRepo.Models.UpdateUserPasswordRequest()
                {
                    User_Id = user.Id,
                    Password_Hash = PasswordHelper.HashPassword(request.NewPassword),
                    Updated_By = ApplicationConstants.SystemUserId
                });
                uow.Commit();
            }

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.PasswordUpdated
            });

            response.Notifications.Add($"Your password has been updated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<GetActivityLogsResponse> GetActivityLogs(GetActivityLogsRequest request)
        {
            var response = new GetActivityLogsResponse();
            var user = await _sessionManager.GetUser();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var logEvents = await uow.SessionRepo.GetSessionLogEventsByUserId(new Repositories.DatabaseRepos.SessionRepo.Models.GetSessionLogEventsByUserIdRequest()
                {
                    User_Id = user.Id
                });

                var eventsLookup = await _cache.SessionEvents();
                response.ActivityLogs = logEvents.Select(l =>
                {
                    var eventEntity = eventsLookup.FirstOrDefault(el => el.Id == l.Event_Id);

                    var type = ActivityTypeEnum.Default;
                    if (eventEntity.Key == "ERROR")
                    {
                        type = ActivityTypeEnum.Error;
                    }

                    var info = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(l.InfoDictionary_JSON))
                    {
                        info = JsonConvert.DeserializeObject<Dictionary<string, string>>(l.InfoDictionary_JSON);
                    }

                    return new ActivityLog()
                    {
                        Date = l.Created_Date,
                        Description = eventEntity.Description,
                        Info = info,
                        Type = type
                    };

                }).ToList();

                uow.Commit();
                return response;
            }
        }

        #endregion
    }
}
