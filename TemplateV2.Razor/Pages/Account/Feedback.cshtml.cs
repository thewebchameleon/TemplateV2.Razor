using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TemplateV2.Models.ServiceModels.Account;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class FeedbackModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _service;

        #endregion

        #region Properties

        [BindProperty]
        public SendFeedbackRequest FormData { get; set; }

        #endregion

        #region Constructors

        public FeedbackModel(IAccountService service)
        {
            _service = service;
            FormData = new SendFeedbackRequest();
        }

        #endregion


        public async Task OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _service.SendFeedback(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToHome();
                }
                AddFormErrors(response);
            }
            return Page();
        }
    }
}
