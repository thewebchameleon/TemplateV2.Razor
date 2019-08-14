using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Services.Managers.Contracts
{
    public interface ICacheManager
    {
        Task<ApplicationConfiguration> Configuration();

        Task<List<PermissionEntity>> Permissions();

        Task<List<RolePermissionEntity>> RolePermissions();

        Task<List<SessionEventEntity>> SessionEvents();

        void Remove(string key);
    }
}
