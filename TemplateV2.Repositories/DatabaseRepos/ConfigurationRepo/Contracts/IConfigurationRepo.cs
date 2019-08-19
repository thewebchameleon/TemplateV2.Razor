using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Repositories.DatabaseRepos.ConfigurationRepo.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Repositories.DatabaseRepos.ConfigurationRepo.Contracts
{
    public interface IConfigurationRepo
    {
        Task<List<ConfigurationEntity>> GetConfigurationItems();

        Task UpdateConfigurationItem(UpdateConfigurationItemRequest request);

        Task<int> CreateConfigurationItem(CreateConfigurationItemRequest request);
    }
}
