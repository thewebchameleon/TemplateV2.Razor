using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TemplateV2.Razor.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-back-url")]
    public class BackButtonTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;

        public BackButtonTagHelper(IHttpContextAccessor httpContextAccessor, IUrlHelper urlHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var refererLink = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(refererLink))
            {
                output.Attributes.SetAttribute("href", _urlHelper.Page("/Index"));
                return;
            }
            output.Attributes.SetAttribute("href", refererLink);
            return;
        }
    }
}
