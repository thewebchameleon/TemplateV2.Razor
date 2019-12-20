using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TemplateV2.Models;

namespace TemplateV2.Razor.Filters
{
    public class MetaTagPageFilter : IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerInstance is PageModel result)
            {
                var metaTags = result.ViewData["MetaTagsViewModel"] as MetaTagsViewModel;
                if (metaTags == null)
                {
                    metaTags = new MetaTagsViewModel();
                }

                metaTags.Title = "TemplateV2.Razor";
                metaTags.Description = "Admin template for small business applications using ASP.NET Core 3.1 + Razor Pages";
                metaTags.Url = context.HttpContext.Request.GetDisplayUrl();
                metaTags.Type = "website";

                result.ViewData["MetaTagsViewModel"] = metaTags;
            }
            await next();
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {

        }
    }

}
