using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.SessionEvents
{
    public class GetSessionEventResponse : ServiceResponse
    {
        public SessionEventEntity SessionEvent { get; set; }
    }
}
