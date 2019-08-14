namespace TemplateV2.Models
{
    public enum TokenTypeEnum : int
    {
        Undefined = 0,
        AccountActivation = 1,
        ResetPassword = 2,
        ForgotPassword = 3
    }

    public enum ActivityTypeEnum : int
    {
        Undefined = 0,
        Default = 1, // standard tick icon
        Error = 2
    }
}
