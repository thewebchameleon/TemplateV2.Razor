using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Permissions
{
    public class GetPermissionsResponse : ServiceResponse
    {
        public List<PermissionEntity> Permissions { get; set; }

        public GetPermissionsResponse()
        {
            Permissions = new List<PermissionEntity>();
        }
    }
}
