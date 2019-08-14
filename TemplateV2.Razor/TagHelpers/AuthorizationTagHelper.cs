using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-authorize")]
    [HtmlTargetElement(Attributes = "asp-permission")]
    [HtmlTargetElement(Attributes = "asp-permissions")]
    public class AuthorizationTagHelper : TagHelper
    {
        private readonly ISessionManager _sessionManager;

        public AuthorizationTagHelper(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            Permissions = new List<string>();
        }

        /// <summary>
        /// Gets or sets the permission name that determines access to the HTML block.
        /// </summary>
        [HtmlAttributeName("asp-permission")]
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the permission name that determines access to the HTML block.
        /// </summary>
        [HtmlAttributeName("asp-permissions")]
        public List<string> Permissions { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var permissions = await _sessionManager.GetPermissions();
            if (permissions == null)
            {
                output.SuppressOutput();
                return;
            }

            if (!string.IsNullOrEmpty(Permission) && !permissions.Select(p => p.Key).Contains(Permission))
            {
                output.SuppressOutput();
                return;
            }

            if (Permissions.Any() && !permissions.Select(p => p.Key).Any(p => Permissions.Contains(p)))
            {
                output.SuppressOutput();
                return;
            }
        }
    }
}
