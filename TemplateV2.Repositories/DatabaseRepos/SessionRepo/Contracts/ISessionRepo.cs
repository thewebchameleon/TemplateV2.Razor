using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Repositories.DatabaseRepos.SessionRepo.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Repositories.DatabaseRepos.SessionRepo.Contracts
{
    public interface ISessionRepo
    {
        Task<SessionEntity> AddUserToSession(AddUserToSessionRequest request);

        Task<SessionEntity> CreateSession(CreateSessionRequest request);

        Task<List<GetSessionsResponse>> GetAdminSessionsByUserId(GetSessionsByUserIdRequest request);

        Task<List<GetSessionsResponse>> GetAdminSessionsByStartDate(GetSessionsByStartDateRequest request);

        Task<List<GetSessionsResponse>> GetAdminSessionsByDate(GetSessionsByDateRequest request);

        Task<int> CreateSessionEvent(CreateSessionEventRequest request);

        Task UpdateSessionEvent(UpdateSessionEventRequest request);

        Task<List<SessionEventEntity>> GetSessionEvents();

        Task<List<SessionLogEventEntity>> GetSessionLogEventsBySessionId(GetSessionLogEventsBySessionIdRequest request);

        Task<List<SessionLogEntity>> GetSessionLogsBySessionId(GetSessionLogsBySessionIdRequest request);

        Task<List<SessionLogEventEntity>> GetSessionLogEventsByUserId(GetSessionLogEventsByUserIdRequest request);

        Task<SessionEntity> GetSessionById(GetSessionByIdRequest request);

        Task<int> CreateSessionLog(CreateSessionLogRequest request);

        Task<int> CreateSessionLogEvent(CreateSessionLogEventRequest request);
    }
}
