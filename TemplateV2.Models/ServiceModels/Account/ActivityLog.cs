using System;
using System.Collections.Generic;

namespace TemplateV2.Models.ServiceModels.Account
{
    public class ActivityLog
    {
        public ActivityTypeEnum Type { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Info { get; set; }

        public DateTime Date { get; set; }

        public ActivityLog()
        {
            Info = new Dictionary<string, string>();
        }
    }
}
