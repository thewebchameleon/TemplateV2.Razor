using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Roles
{
    public class GetRoleResponse : ServiceResponse
    {
        public RoleEntity Role { get; set; }

        public List<PermissionEntity> Permissions { get; set; }

        public GetRoleResponse()
        {
            Permissions = new List<PermissionEntity>();
        }
    }
}
