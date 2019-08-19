using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Roles;

namespace TemplateV2.Services.Admin.Contracts
{
    public interface IRoleService
    {
        Task<GetRolesResponse> GetRoles();

        Task<DisableRoleResponse> DisableRole(DisableRoleRequest request);

        Task<EnableRoleResponse> EnableRole(EnableRoleRequest request);

        Task<GetRoleResponse> GetRole(GetRoleRequest request);

        Task<UpdateRoleResponse> UpdateRole(UpdateRoleRequest request);

        Task<CreateRoleResponse> CreateRole(CreateRoleRequest request);
    }
}
