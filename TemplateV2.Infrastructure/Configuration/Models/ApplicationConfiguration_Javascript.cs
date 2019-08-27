using System;
using System.Collections.Generic;
using System.Linq;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Infrastructure.Configuration.Models
{
    public class ApplicationConfiguration_Javascript
    {
        #region Instance Fields

        public readonly List<ConfigurationEntity> Items;

        #endregion

        #region Properties

        public bool Auto_Logout_Is_Enabled
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Auto_Logout_Is_Enabled);
                return item.Boolean_Value.Value;
            }
        }

        public int Toast_Delay_Seconds
        {
            get
            {
                var item = GetItem(ConfigurationKeys.Toast_Delay_Seconds);
                return item.Int_Value.Value;
            }
        }

        public int Idle_Timeout_Seconds
        {
            get
            {
                return ApplicationConstants.SessionTimeoutSeconds;
            }
        }

        public int Idle_Timeout_Modal_Seconds
        {
            get
            {
                return ApplicationConstants.SessionModalTimeoutSeconds;
            }
        }

        #endregion

        #region Constructors

        public ApplicationConfiguration_Javascript(List<ConfigurationEntity> items)
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
