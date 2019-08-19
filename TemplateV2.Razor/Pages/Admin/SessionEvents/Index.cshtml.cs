using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageSessionEventsModel : PageModel
    {
        #region Private Fields

        private readonly ISessionService _sessionService;

        #endregion

        #region Properties

        public List<SessionEventEntity> SessionEvents { get; set; }

        #endregion

        #region Constructors

        public ManageSessionEventsModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            SessionEvents = new List<SessionEventEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _sessionService.GetSessionEvents();
            SessionEvents = response.SessionEvents;
        }
    }
}
