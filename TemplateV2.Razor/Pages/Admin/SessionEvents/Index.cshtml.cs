using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageSessionEventsModel : PageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        public List<SessionEventEntity> SessionEvents { get; set; }

        #endregion

        #region Constructors

        public ManageSessionEventsModel(IAdminService adminService)
        {
            _adminService = adminService;
            SessionEvents = new List<SessionEventEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _adminService.GetSessionEventManagement();
            SessionEvents = response.SessionEvents;
        }
    }
}
