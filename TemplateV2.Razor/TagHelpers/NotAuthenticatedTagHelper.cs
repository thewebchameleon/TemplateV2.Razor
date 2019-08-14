using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-not-authenticated")]
    public class NotAuthenticatedTagHelper : TagHelper
    {
        private readonly ISessionManager _sessionManager;

        public NotAuthenticatedTagHelper(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _sessionManager.GetUser();
            if (user == null)
            {
                return;
            }
            output.SuppressOutput();
            return;
        }
    }
}
