using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Models.ServiceModels.Admin.Sessions;

namespace TemplateV2.Services.Admin.Contracts
{
    public interface ISessionService
    {
        Task<GetSessionsResponse> GetSessions(GetSessionsRequest request);

        Task<GetSessionResponse> GetSession(GetSessionRequest request);

        Task<GetSessionLogsResponse> GetSessionLogs(GetSessionLogsRequest request);

        Task<GetSessionEventsResponse> GetSessionEvents();

        Task<GetSessionEventResponse> GetSessionEvent(GetSessionEventRequest request);

        Task<UpdateSessionEventResponse> UpdateSessionEvent(UpdateSessionEventRequest request);

        Task<CreateSessionEventResponse> CreateSessionEvent(CreateSessionEventRequest request);
    }
}
