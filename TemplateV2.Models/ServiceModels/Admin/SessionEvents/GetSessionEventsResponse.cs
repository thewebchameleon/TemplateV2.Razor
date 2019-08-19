using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.SessionEvents
{
    public class GetSessionEventsResponse : ServiceResponse
    {
        public List<SessionEventEntity> SessionEvents { get; set; }

        public GetSessionEventsResponse()
        {
            SessionEvents = new List<SessionEventEntity>();
        }
    }
}
