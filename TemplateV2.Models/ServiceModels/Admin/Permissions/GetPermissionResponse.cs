using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Permissions
{
    public class GetPermissionResponse : ServiceResponse
    {
        public PermissionEntity Permission { get; set; }
    }
}
