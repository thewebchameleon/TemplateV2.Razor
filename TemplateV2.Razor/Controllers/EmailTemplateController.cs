using Microsoft.AspNetCore.Mvc;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Controllers
{
    [Route("[controller]/[action]")]
    public class EmailTemplateController : BaseController
    {
        #region Instance Fields

        private readonly IAccountService _service;

        #endregion

        #region Constructors

        public EmailTemplateController(
            IAccountService service)
        {
            _service = service;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IActionResult AccountActivation()
        {
            return View();
        }

        #endregion
    }
}
