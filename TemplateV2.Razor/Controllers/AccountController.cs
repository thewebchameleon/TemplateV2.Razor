using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        #region Instance Fields

        private readonly IAccountService _service;

        #endregion

        #region Constructors

        public AccountController(
            IAccountService service)
        {
            _service = service;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public async Task <IActionResult> Logout()
        {
            await _service.Logout();
            return RedirectToHome();
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAccount(string token)
        {
            var response = await _service.ActivateAccount(new Models.ServiceModels.Account.ActivateAccountRequest()
            {
                Token = token
            });

            AddNotifications(response);
            return RedirectToHome();
        }

        #endregion
    }
}
