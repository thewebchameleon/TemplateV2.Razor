namespace TemplateV2.Infrastructure.Configuration
{
    /// <summary>
    /// These constants map to the [Key] field of each configuration item
    /// </summary>
    public class ConfigurationKeys
    {
        public const string Session_Logging_Is_Enabled = "SESSION_LOGGING_IS_ENABLED";
        public const string Home_Promo_Banner_Is_Enabled = "HOME_PROMO_BANNER_IS_ENABLED";
        public const string Account_Lockout_Expiry_Minutes = "ACCOUNT_LOCKOUT_EXPIRY_MINUTES";
        public const string Max_Login_Attempts = "MAX_LOGIN_ATTEMPTS";
        public const string System_From_Email_Address = "SYSTEM_FROM_EMAIL_ADDRESS";
        public const string Contact_Email_Address = "CONTACT_EMAIL_ADDRESS";
    }
}
