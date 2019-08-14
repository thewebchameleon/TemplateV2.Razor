using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateV2.Models.EmailTemplates
{
    public class ResetPasswordTemplate : BaseTemplate
    {
        public override string Subject => "Reset password request";

        public string ResetPasswordUrl { get; set; }

        #region Constructors

        public ResetPasswordTemplate(string body) : base(body)
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
