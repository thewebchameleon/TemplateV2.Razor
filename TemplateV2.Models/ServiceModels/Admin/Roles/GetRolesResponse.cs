using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Roles
{
    public class GetRolesResponse : ServiceResponse
    {
        public List<RoleEntity> Roles { get; set; }

        public GetRolesResponse()
        {
            Roles = new List<RoleEntity>();
        }
    }
}
