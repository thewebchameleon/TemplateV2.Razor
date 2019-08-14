using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TemplateV2.Models.ServiceModels.Admin.Permissions
{
    public class CreatePermissionRequest : IValidatableObject
    {
        [Required]
        public string Key { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Regex.IsMatch(Key, @"^[A-Z0-9_]+$"))
            {
                yield return new ValidationResult("Permission key is invalid, please ensure it is uppercase, contains only letters and numbers and uses underscores instead of spaces");
            }
        }
    }
}
