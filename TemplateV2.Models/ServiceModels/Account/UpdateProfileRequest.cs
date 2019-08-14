using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Account
{
    public class UpdateProfileRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile number")]
        public string MobileNumber { get; set; }
    }
}
