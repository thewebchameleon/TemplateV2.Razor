using Microsoft.AspNetCore.Mvc;

namespace TemplateV2.Razor.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class Error401Model : BasePageModel
    {
        public void OnGet()
        {

        }
    }
}
