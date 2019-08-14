using TemplateV2.Razor.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TemplateV2.Razor.Attributes
{
    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        public AuthorizePermissionAttribute(string key) : base(typeof(PermissionRequirementFilter))
        {
            Arguments = new object[] { key };
        }
    }
}