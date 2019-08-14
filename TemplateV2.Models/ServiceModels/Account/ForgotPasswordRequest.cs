using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
}
