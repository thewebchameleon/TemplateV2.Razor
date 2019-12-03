using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Dashboard;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Services
{
    public class DashboardService : IDashboardService
    {
        #region Instance Fields

        private readonly IUnitOfWorkFactory _uowFactory;

        #endregion

        #region Constructor

        public DashboardService(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        #endregion

        #region Public Methods

        public async Task<GetIndexDashBoardResponse> GetIndexDashBoard()
        {
            var response = new GetIndexDashBoardResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var dashboardResponse = await uow.DashboardRepo.GetDashboard();

                response = new GetIndexDashBoardResponse()
                {
                    TotalConfigItems = dashboardResponse.TotalConfigItems,
                    TotalRoles = dashboardResponse.TotalRoles,
                    TotalSessions = dashboardResponse.TotalSessions,
                    TotalUsers = dashboardResponse.TotalUsers
                };
            }

            return response;
        }

        #endregion
    }
}
