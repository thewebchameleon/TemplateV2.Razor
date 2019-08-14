using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Account
{
    public class UpdatePasswordRequest : IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string NewPasswordConfirm { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != NewPasswordConfirm)
            {
                yield return new ValidationResult("Passwords do not match");
            }
        }
    }
}
