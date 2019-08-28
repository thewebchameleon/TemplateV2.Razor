using System;
using System.Collections.Generic;

namespace TemplateV2.Infrastructure.Configuration.Models
{
    public class Client_Configuration
    {
        public string Key { get; set; }

        public string Description { get; set; }

        public bool? Boolean_Value { get; set; }

        public DateTime? DateTime_Value { get; set; }

        public DateTime? Date_Value { get; set; }

        public TimeSpan? Time_Value { get; set; }

        public decimal? Decimal_Value { get; set; }

        public int? Int_Value { get; set; }

        public decimal? Money_Value { get; set; }

        public string String_Value { get; set; }
    }

    public class ApplicationConfiguration_Javascript
    {
        #region Instance Fields

        public readonly List<Client_Configuration> Items;

        #endregion

        #region Constructors

        public ApplicationConfiguration_Javascript(List<Client_Configuration> items)
        {
            Items = items;
        }

        #endregion
    }
}
