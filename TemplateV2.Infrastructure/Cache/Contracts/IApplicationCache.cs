using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Infrastructure.Cache.Contracts
{
    public interface IApplicationCache
    {
        Task<ApplicationConfiguration> Configuration();

        Task<List<RoleEntity>> Roles();

        Task<List<UserRoleEntity>> UserRoles();

        Task<List<RolePermissionEntity>> RolePermissions();

        Task<List<PermissionEntity>> Permissions();

        Task<List<SessionEventEntity>> SessionEvents();

        void Remove(string key);
    }
}
