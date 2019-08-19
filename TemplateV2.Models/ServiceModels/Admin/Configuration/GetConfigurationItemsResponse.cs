using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Configuration
{
    public class GetConfigurationItemsResponse
    {
        public List<ConfigurationEntity> ConfigurationItems { get; set; }

        public GetConfigurationItemsResponse()
        {
            ConfigurationItems = new List<ConfigurationEntity>();
        }
    }
}
