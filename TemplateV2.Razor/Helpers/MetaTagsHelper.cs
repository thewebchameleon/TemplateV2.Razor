using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TemplateV2.Common.Constants;
using TemplateV2.Models;

namespace TemplateV2.Razor.Helpers
{
    public static class MetaTagsHelper
    {
        public static void SetMeta(this ViewDataDictionary viewData, string title, string? description = null)
        {
            var model = viewData[ViewDataConstants.OpenGraphViewModel] as OpenGraphViewModel;

            if (model == null)
            {
                model = new OpenGraphViewModel();
            }

            model.Title = title;
            if (description != null)
            {
                model.Description = description;
            }
            viewData[ViewDataConstants.OpenGraphViewModel] = model;
        }
    }
}
