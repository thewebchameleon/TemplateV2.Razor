using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TemplateV2.Razor.Controllers
{
    [Route("[controller]/[action]")]
    public class DiagnosticsController : BaseController
    {
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DiagnosticsController>();
        }

        [HttpGet]
        public IActionResult Ping()
        {
            _logger.LogInformation("App has been pinged");
            return Ok("Pong");
        }
    }
}