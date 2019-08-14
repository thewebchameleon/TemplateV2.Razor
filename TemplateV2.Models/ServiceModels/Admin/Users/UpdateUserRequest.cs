using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Admin.Users
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }

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

        [Display(Name = "Roles")]
        public List<int> RoleIds { get; set; }

        public UpdateUserRequest()
        {
            RoleIds = new List<int>();
        }
    }
}
