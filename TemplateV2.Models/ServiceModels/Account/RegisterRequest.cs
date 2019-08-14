using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels
{
    public class RegisterRequest : IValidatableObject
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != PasswordConfirm)
            {
                yield return new ValidationResult("Passwords do not match");
            }
        }
    }
}
