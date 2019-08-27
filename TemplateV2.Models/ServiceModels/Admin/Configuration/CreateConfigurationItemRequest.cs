using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace TemplateV2.Models.ServiceModels.Admin.Configuration
{
    public class CreateConfigurationItemRequest : IValidatableObject
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Is Client Side")]
        public bool IsClientSide { get; set; }

        [Display(Name = "Boolean value")]
        public bool? BooleanValue { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date and time value")]
        public DateTime? DateTimeValue { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date value")]
        public DateTime? DateValue { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Time value")]
        public TimeSpan? TimeValue { get; set; }

        [Display(Name = "Decimal value")]
        public decimal? DecimalValue { get; set; }

        [Display(Name = "Integer value")]
        public int? IntValue { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Money value")]
        public decimal? MoneyValue { get; set; }

        [Display(Name = "String value")]
        public string StringValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int populatedFieldCount = 0;

            if (BooleanValue != null)
            {
                populatedFieldCount++;
            }

            if (DateTimeValue != null)
            {
                populatedFieldCount++;
            }

            if (DateValue != null)
            {
                populatedFieldCount++;
            }

            if (TimeValue != null)
            {
                populatedFieldCount++;
            }

            if (DecimalValue != null)
            {
                populatedFieldCount++;
            }

            if (IntValue != null)
            {
                populatedFieldCount++;
            }

            if (MoneyValue != null)
            {
                populatedFieldCount++;
            }

            if (StringValue != null)
            {
                populatedFieldCount++;
            }

            if (populatedFieldCount == 0)
            {
                yield return new ValidationResult("Please configure at least 1 value");
            }

            if (populatedFieldCount > 1)
            {
                yield return new ValidationResult("You may only have 1 configured value");
            }

            if (!Regex.IsMatch(Key, @"^[A-Z0-9_]+$"))
            {
                yield return new ValidationResult("Configuration key is invalid, please ensure it is uppercase, contains only letters and numbers and uses underscores instead of spaces");
            }
        }
    }
}
