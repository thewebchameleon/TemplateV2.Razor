using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class CreateSessionEventModel : BasePageModel
    {
        #region Private Fields

        private readonly ISessionService _sessionService;

        #endregion

        #region Properties

        [BindProperty]
        public CreateSessionEventRequest FormData { get; set; }

        #endregion

        #region Constructors

        public CreateSessionEventModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #endregion

        public async Task OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _sessionService.CreateSessionEvent(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/SessionEvents/Index");
                }
                AddFormErrors(response);
            }
            return Page();
        }
    }
}
