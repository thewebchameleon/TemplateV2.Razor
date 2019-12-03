using Microsoft.AspNetCore.Mvc;

namespace TemplateV2.Razor.Controllers
{
    [Route("[controller]/[action]")]
    public class DiagnosticsController : BaseController
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}