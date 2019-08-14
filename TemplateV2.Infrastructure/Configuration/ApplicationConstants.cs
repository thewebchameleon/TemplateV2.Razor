using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TemplateV2.Infrastructure.Configuration
{
    public static class ApplicationConstants
    {
        public const int SystemUserId = 1;
        public const string CultureInfo = "en-ZA";
        public const int SessionTimeoutSeconds = 60 * 10; // 10 minutes
        public const int ResponseCachingSeconds = 60 * 60 * 24; // 24 hours

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
