using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Permissions
{
    public class GetPermissionManagementResponse : ServiceResponse
    {
        public List<PermissionEntity> Permissions { get; set; }

        public GetPermissionManagementResponse()
        {
            Permissions = new List<PermissionEntity>();
        }
    }
}
