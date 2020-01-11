using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;

namespace TemplateV2.Infrastructure.Configuration
{
    public static class ApplicationConstants
    {
        public const int SystemUserId = 1;
        public const int StaticFileCachingSeconds = 60 * 60 * 24 * 365; // 1 year

        // the amount of seconds before the session times out
        public const int SessionTimeoutSeconds = 60 * 10; // 10 minutes

        // the auto-logout popup will appear after X seconds before the sessions times out (X = 30 seconds)
        public const int SessionModalTimeoutSeconds = SessionTimeoutSeconds - 30;

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

                cultureInfo_ZA.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                cultureInfo_ZA.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy hh:mm tt";

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
