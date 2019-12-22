using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditSessionEventModel : BasePageModel
    {
        #region Private Fields

        private readonly ISessionService _sessionService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdateSessionEventRequest FormData { get; set; }

        public string? Key { get; set; }

        #endregion

        #region Constructors

        public EditSessionEventModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            FormData = new UpdateSessionEventRequest();
        }

        #endregion

        public async Task<IActionResult> OnGet()
        {
            var response = await _sessionService.GetSessionEvent(new GetSessionEventRequest()
            {
                Id = Id
            });

            if (!response.IsSuccessful)
            {
                return NotFound();
            }

            Key = response.SessionEvent.Key;
            FormData = new UpdateSessionEventRequest()
            {
                Id = response.SessionEvent.Id,
                Description = response.SessionEvent.Description
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                FormData.Id = Id;
                var response = await _sessionService.UpdateSessionEvent(FormData);
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
