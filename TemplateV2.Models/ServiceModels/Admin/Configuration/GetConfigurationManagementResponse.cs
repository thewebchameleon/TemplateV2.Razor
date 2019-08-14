using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Configuration
{
    public class GetConfigurationManagementResponse
    {
        public List<ConfigurationEntity> ConfigurationItems { get; set; }

        public GetConfigurationManagementResponse()
        {
            ConfigurationItems = new List<ConfigurationEntity>();
        }
    }
}
