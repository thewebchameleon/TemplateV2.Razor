using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateV2.Models.EmailTemplates
{
    public class SendFeedbackTemplate : BaseTemplate
    {
        public override string Subject => $"Feedback received";

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Message { get; set; }

        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? EmailAddress : Name;
            }
        }

        #region Constructors

        public SendFeedbackTemplate(string body, string applicationUrl) : base(body, applicationUrl)
        {
        }

        #endregion

        #region Public Methods

        public override string GetHTMLContent()
        {
            // perform replacements
            _body = _body.Replace("{{DisplayName}}", DisplayName);
            _body = _body.Replace("{{Message}}", Message);

            return base.GetHTMLContent();
        }

        #endregion
    }
}
