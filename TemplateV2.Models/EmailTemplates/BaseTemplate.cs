using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateV2.Models.EmailTemplates
{
    public abstract class BaseTemplate
    {
        public string _body;

        public BaseTemplate(string body)
        {
            _body = body;
        }

        public abstract string Subject { get; }

        public string ApplicationUrl { get; set; }

        public virtual string GetHTMLContent()
        {
            // default replacements

            _body = _body.Replace("{{Application_Url}}", ApplicationUrl);
            return _body;
        }
    }
}
