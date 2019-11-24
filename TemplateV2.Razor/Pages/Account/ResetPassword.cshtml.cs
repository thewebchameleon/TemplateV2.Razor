using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TemplateV2.Models.ServiceModels.Account;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ResetPasswordModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Properties

        [BindProperty]
        public ResetPasswordRequest FormData { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        #endregion

        #region Constructors

        public ResetPasswordModel(IAccountService accountService)
        {
            _accountService = accountService;
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
                var response = await _accountService.ResetPassword(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToHome();
                }
                AddFormErrors(response);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
