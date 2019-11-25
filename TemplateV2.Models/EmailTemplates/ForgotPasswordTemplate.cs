namespace TemplateV2.Models.EmailTemplates
{
    public class ForgotPasswordTemplate : BaseTemplate
    {
        public override string Subject => "Forgot password request";

        public string ResetPasswordUrl { get; set; }

        #region Constructors

        public ForgotPasswordTemplate(string body, string applicationUrl) : base(body, applicationUrl)
        {
        }

        #endregion

        #region Public Methods

        public override string GetHTMLContent()
        {
            // perform replacements
            _body = _body.Replace("{{ResetPassword_Url}}", ResetPasswordUrl);

            return base.GetHTMLContent();
        }

        #endregion
    }
}
