using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.SessionEvents
{
    public class GetSessionEventManagementResponse : ServiceResponse
    {
        public List<SessionEventEntity> SessionEvents { get; set; }

        public GetSessionEventManagementResponse()
        {
            SessionEvents = new List<SessionEventEntity>();
        }
    }
}
