using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateV2.Models.EmailTemplates
{
    public class ContactMessageTemplate : BaseTemplate
    {
        public override string Subject => $"General enquiry from {Name}";

        public string Name { get; set; }

        public string Message { get; set; }

        #region Constructors

        public ContactMessageTemplate(string body) : base(body)
        {
        }

        #endregion

        #region Public Methods

        public override string GetHTMLContent()
        {
            // perform replacements
            _body = _body.Replace("{{Name}}", Name);
            _body = _body.Replace("{{Message}}", Message);

            return base.GetHTMLContent();
        }

        #endregion
    }
}
