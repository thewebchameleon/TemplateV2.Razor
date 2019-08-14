using System;
using TemplateV2.Common.Constants;

namespace TemplateV2.Models.DomainModels
{
    public class ConfigurationEntity : BaseEntity
    {
        public string Key { get; set; }

        public string Description { get; set; }

        public bool? Boolean_Value { get; set; }

        public DateTime? DateTime_Value { get; set; }

        public decimal? Decimal_Value { get; set; }

        public int? Int_Value { get; set; }

        public decimal? Money_Value { get; set; }

        public string String_Value { get; set; }
    }

    public static class ConfigurationEntityExtensions
    {
        public static string GetDisplayValue(this ConfigurationEntity item)
        {
            if (item.Boolean_Value != null)
            {
                return item.Boolean_Value.ToString();
            }

            if (item.DateTime_Value != null)
            {
                // Friday, 29 May 2015 5:50 AM
                return item.DateTime_Value.Value.ToString("dddd, dd MMMM yyyy hh:mm tt");
            }

            if (item.Decimal_Value != null)
            {
                return item.Decimal_Value.Value.ToString();
            }

            if (item.Int_Value != null)
            {
                return item.Int_Value.Value.ToString();
            }

            if (item.Money_Value != null)
            {
                return item.Money_Value.Value.ToString(CurrencyFormatConstants.Decimal);
            }

            if (item.String_Value != null)
            {
                return item.String_Value;
            }

            return string.Empty;
        }
    }
}
