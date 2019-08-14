using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Admin.Sessions
{
    public class GetSessionsRequest : IValidatableObject
    {
        public bool? Last24Hours { get; set; }

        [Display(Name = "Days in the past")]
        public int? LastXDays { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Day { get; set; }

        [Display(Name = "User ID")]
        public int? UserId { get; set; }

        public string Username { get; set; }

        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // if any user fields are filled out
            var userFieldCount = 0;
            if (UserId.HasValue)
            {
                userFieldCount++;
            }

            if (!string.IsNullOrEmpty(Username))
            {
                userFieldCount++;
            }

            if (!string.IsNullOrEmpty(MobileNumber))
            {
                userFieldCount++;
            }

            if (!string.IsNullOrEmpty(EmailAddress))
            {
                userFieldCount++;
            }

            if (userFieldCount > 1)
            {
                yield return new ValidationResult("You may only search by a single field");
            }
        }
    }
}
