using System.Threading.Tasks;
using TemplateV2.Repositories.DatabaseRepos.DashboardRepo.Models;

namespace TemplateV2.Repositories.DatabaseRepos.DashboardRepo.Contracts
{
    public interface IDashboardRepo
    {
        Task<GetDashboardResponse> GetDashboard();
    }
}
