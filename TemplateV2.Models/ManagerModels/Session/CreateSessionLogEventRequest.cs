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

    public static class CreateSessionLogEventRequest_Extensions
    {
        public static CreateSessionLogEventRequest AddInfo(this CreateSessionLogEventRequest request, string key, string value)
        {
            request.Info.Add(key, value);
            return request;
        }
    }
}
