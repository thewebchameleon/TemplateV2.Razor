using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ForgotPasswordModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _service;

        #endregion

        #region Properties

        [BindProperty]
        public ForgotPasswordRequest FormData { get; set; }

        #endregion

        #region Constructors

        public ForgotPasswordModel(IAccountService service)
        {
            _service = service;
        }

        #endregion

        public void OnGet()
        {
            FormData = new ForgotPasswordRequest();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _service.ForgotPassword(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToHome();
                }
                else
                {
                    AddFormErrors(response);
                }
            }
            return Page();
        }
    }
}
