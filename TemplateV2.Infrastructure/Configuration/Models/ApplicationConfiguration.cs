using System;
using System.Collections.Generic;
using System.Linq;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Infrastructure.Configuration.Models
{
    public class ApplicationConfiguration
    {
        #region Instance Fields

        public readonly List<ConfigurationEntity> Items;

        #endregion

        #region Properties

        public bool Session_Logging_Is_Enabled
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Session_Logging_Is_Enabled);
                return item.Boolean_Value.Value;
            }
        }

        public bool Home_Promo_Banner_Is_Enabled
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Home_Promo_Banner_Is_Enabled);
                return item.Boolean_Value.Value;
            }
        }

        public int Max_Login_Attempts
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Max_Login_Attempts);
                return item.Int_Value.Value;
            }
        }

        public int Account_Lockout_Expiry_Minutes
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Account_Lockout_Expiry_Minutes);
                return item.Int_Value.Value;
            }
        }

        public string System_From_Email_Address
        {
            get
            {
                var item = GetItem(ConfigurationKeys.System_From_Email_Address);
                return item.String_Value;
            }
        }

        public string Contact_Email_Address
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Contact_Email_Address);
                return item.String_Value;
            }
        }

        #endregion

        #region Constructors

        public ApplicationConfiguration(List<ConfigurationEntity> items)
        {
            Items = items;
        }

        #endregion

        #region Private Methods

        private ConfigurationEntity GetItem(string key)
        {
            var configItem = Items.FirstOrDefault(c => c.Key == key);
            if (configItem == null)
            {
                throw new Exception($"Configuration item with key '{key}' could not be found");
            }
            return configItem;
        }

        #endregion
    }
}
