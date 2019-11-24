using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Controllers
{
    [Route("[controller]/[action]")]
    public class SessionController : BaseController
    {
        #region Instance Fields


        #endregion

        #region Constructors

        public SessionController()
        {
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Heartbeat()
        {
            return Ok();
        }

        #endregion
    }
}
