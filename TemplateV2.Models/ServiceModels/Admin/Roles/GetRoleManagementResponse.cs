using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Roles
{
    public class GetRoleManagementResponse : ServiceResponse
    {
        public List<RoleEntity> Roles { get; set; }

        public GetRoleManagementResponse()
        {
            Roles = new List<RoleEntity>();
        }
    }
}
