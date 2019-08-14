using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels
{
    public class DuplicateUserCheckRequest : IValidatableObject
    {
        /// <summary>
        /// If we are updating, we need to exclude the user in our check
        /// </summary>
        public int? UserId { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

        public string Username { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(EmailAddress) &&
                string.IsNullOrEmpty(MobileNumber) &&
                string.IsNullOrEmpty(Username)
            )
            {
                yield return new ValidationResult("Please provide at least 1 field to check for duplicate users");
            }
        }
    }
}
