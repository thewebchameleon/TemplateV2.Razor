using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;

namespace TemplateV2.Infrastructure.Configuration
{
    public static class ApplicationConstants
    {
        public const int SystemUserId = 1;
        public const int SessionTimeoutSeconds = 60 * 10; // 10 minutes
        public const int ResponseCachingSeconds = 60 * 60 * 24; // 24 hours

        public static CultureInfo Culture
        {
            get
            {
                var cultureInfo_ZA = new CultureInfo("en-ZA");

                cultureInfo_ZA.NumberFormat.CurrencySymbol = "R";
                cultureInfo_ZA.NumberFormat.CurrencyGroupSeparator = " ";
                cultureInfo_ZA.NumberFormat.CurrencyDecimalSeparator = ".";

                cultureInfo_ZA.NumberFormat.NumberGroupSeparator = " ";
                cultureInfo_ZA.NumberFormat.NumberDecimalSeparator = ".";

                return cultureInfo_ZA;
            }
        }
        
        public static CookieBuilder SecureNamelessCookie
        {
            get
            {
                return new CookieBuilder()
                {
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.Always,
                    IsEssential = true,
                    HttpOnly = false
                };
            }
        }

        /// <summary>
        /// a list of field names that may contain user-sensitive data to be used in 
        /// </summary>
        public static List<string> ObfuscatedActionArgumentFields
        {
            get
            {
                return new List<string>()
                {
                    "Password", "password"
                };
            }
        }
    }
}
