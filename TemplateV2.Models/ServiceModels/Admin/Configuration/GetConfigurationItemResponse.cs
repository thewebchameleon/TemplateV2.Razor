using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Configuration
{
    public class GetConfigurationItemResponse : ServiceResponse
    {
        public ConfigurationEntity ConfigurationItem { get; set; }
    }
}
