using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TemplateV2.Models.ServiceModels.Account;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ResetPasswordModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _service;

        #endregion

        #region Properties

        [BindProperty]
        public ResetPasswordRequest FormData { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        #endregion

        #region Constructors

        public ResetPasswordModel(IAccountService service)
        {
            _service = service;
        }

        #endregion

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            FormData.Token = Token;

            if (ModelState.IsValid)
            {
                var response = await _service.ResetPassword(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToLogin();
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
