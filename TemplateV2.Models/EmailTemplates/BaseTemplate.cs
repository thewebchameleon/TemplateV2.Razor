namespace TemplateV2.Models.EmailTemplates
{
    public abstract class BaseTemplate
    {
        public string _body;
        public string _applicationUrl;

        public BaseTemplate(string body, string applicationUrl)
        {
            _body = body;
            _applicationUrl = applicationUrl;
        }

        public abstract string Subject { get; }

        public virtual string GetHTMLContent()
        {
            // default replacements

            _body = _body.Replace("{{Application_Url}}", _applicationUrl);
            return _body;
        }
    }
}
