using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Configuration;

namespace TemplateV2.Services.Admin.Contracts
{
    public interface IConfigurationService
    {
        Task<GetConfigurationItemsResponse> GetConfigurationItems();

        Task<GetConfigurationItemResponse> GetConfigurationItem(GetConfigurationItemRequest request);

        Task<UpdateConfigurationItemResponse> UpdateConfigurationItem(UpdateConfigurationItemRequest request);

        Task<CreateConfigurationItemResponse> CreateConfigurationItem(CreateConfigurationItemRequest request);
    }
}
