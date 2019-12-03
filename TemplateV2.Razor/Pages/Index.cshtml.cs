using System.Threading.Tasks;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class IndexModel : BasePageModel
    {
        #region Private Fields

        private readonly IDashboardService _dashboardService;

        #endregion

        #region Properties

        public int TotalSessions { get; set; }

        public int TotalUsers { get; set; }

        public int TotalRoles { get; set; }

        public int TotalConfigItems { get; set; }

        #endregion

        #region Constructors

        public IndexModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _dashboardService.GetIndexDashBoard();
            if (response.IsSuccessful)
            {
                TotalUsers = response.TotalUsers;
                TotalSessions = response.TotalSessions;
                TotalRoles = response.TotalRoles;
                TotalConfigItems = response.TotalConfigItems;
            }
            AddNotifications(response);
        }
    }
}
