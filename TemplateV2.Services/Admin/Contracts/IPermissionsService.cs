using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Permissions;

namespace TemplateV2.Services.Admin.Contracts
{
    public interface IPermissionsService
    {
        Task<GetPermissionsResponse> GetPermissions();

        Task<GetPermissionResponse> GetPermission(GetPermissionRequest request);

        Task<UpdatePermissionResponse> UpdatePermission(UpdatePermissionRequest request);

        Task<CreatePermissionResponse> CreatePermission(CreatePermissionRequest request);
    }
}
