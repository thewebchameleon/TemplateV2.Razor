using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Admin.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Permissions")]
        public List<CheckboxItemSelection> PermissionIds { get; set; }

        public CreateRoleRequest()
        {
            PermissionIds = new List<CheckboxItemSelection>();
        }
    }
}
