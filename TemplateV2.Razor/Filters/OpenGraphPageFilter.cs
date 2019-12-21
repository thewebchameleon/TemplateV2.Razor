using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TemplateV2.Common.Constants;
using TemplateV2.Models;

namespace TemplateV2.Razor.Filters
{
    public class OpenGraphPageFilter : IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            // do something before the action executes
            var resultContext = await next();
            // do something after the action executes; resultContext.Result will be set

            if (resultContext.HandlerInstance is PageModel result)
            {
                var metaTags = result.ViewData[ViewDataConstants.OpenGraphViewModel] as OpenGraphViewModel;
                if (metaTags == null)
                {
                    metaTags = new OpenGraphViewModel();
                }

                if (!string.IsNullOrEmpty(metaTags.Title))
                {
                    metaTags.Title = "TemplateV2.Razor";
                }
                if (!string.IsNullOrEmpty(metaTags.Description))
                {
                    metaTags.Description = "Admin template for small business applications using ASP.NET Core 3.1 + Razor Pages";
                }
                metaTags.Url = context.HttpContext.Request.GetDisplayUrl();
                metaTags.Type = "website";

                result.ViewData[ViewDataConstants.OpenGraphViewModel] = metaTags;
            }
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {

        }
    }

    public static class OpenGraphHelper
    {
        public static void Set(string title, string description = null)
        {

        }
    }
}
