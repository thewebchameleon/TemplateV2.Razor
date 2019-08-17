using System.Collections.Generic;

namespace TemplateV2.Models.ManagerModels.Session
{
    public class CreateSessionLogEventRequest
    {
        public string EventKey { get; set; }

        public Dictionary<string, string> Info { get; set; }

        public CreateSessionLogEventRequest()
        {
            Info = new Dictionary<string, string>();
        }
    }
}
