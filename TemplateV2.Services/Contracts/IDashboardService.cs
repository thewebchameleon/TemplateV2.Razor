using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Dashboard;

namespace TemplateV2.Services.Contracts
{
    public interface IDashboardService
    {
        Task<GetIndexDashBoardResponse> GetIndexDashBoard();
    }
}
