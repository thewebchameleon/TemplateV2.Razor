using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Common.Notifications;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Models.ServiceModels.Admin.Sessions;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Services.Admin
{
    public class SessionService : ISessionService
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

        public SessionService(
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
    }
}
