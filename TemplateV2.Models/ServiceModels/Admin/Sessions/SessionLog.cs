using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Sessions
{
    public class SessionLog
    {
        public SessionLogEntity Entity { get; set; }

        public List<SessionLogEvent> Events { get; set; }
    }
}
