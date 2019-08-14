using System.Linq;
using TemplateV2.Common.Notifications;

namespace TemplateV2.Models.ServiceModels
{
    public class ServiceResponse : IServiceResponse
    {
        public NotificationCollection Notifications { get; set; }

        public bool IsSuccessful { get { return !Notifications.Any(n => n.Type == NotificationTypeEnum.Error); } }

        public ServiceResponse()
        {
            Notifications = new NotificationCollection();
        }
    }
}
