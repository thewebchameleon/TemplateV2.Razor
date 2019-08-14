using System.Collections.Generic;

namespace TemplateV2.Models.ServiceModels.Admin.Sessions
{
    public class GetSessionLogsResponse : ServiceResponse
    {
        public List<SessionLog> Logs { get; set; }

        public GetSessionLogsResponse()
        {
            Logs = new List<SessionLog>();
        }
    }
}
