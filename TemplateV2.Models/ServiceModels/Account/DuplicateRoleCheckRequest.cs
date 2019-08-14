using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels
{
    public class DuplicateRoleCheckRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
