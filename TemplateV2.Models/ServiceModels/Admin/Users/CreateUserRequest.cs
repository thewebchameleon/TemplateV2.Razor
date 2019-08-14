using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Admin.Users
{
    public class CreateUserRequest : IValidatableObject
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Mobile number")]
        public string MobileNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Roles")]
        public List<int> RoleIds { get; set; }

        public CreateUserRequest()
        {
            RoleIds = new List<int>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != PasswordConfirm)
            {
                yield return new ValidationResult("Password and confirm password do not match");
            }
        }
    }
}
