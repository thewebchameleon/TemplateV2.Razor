using TemplateV2.Common.Notifications;

namespace TemplateV2.Models.ServiceModels
{
    public interface IServiceResponse
    {
        NotificationCollection Notifications { get; set; }
    }
}
