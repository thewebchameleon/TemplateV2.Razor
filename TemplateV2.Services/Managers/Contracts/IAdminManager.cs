using System.Threading.Tasks;
using TemplateV2.Models.ManagerModels.Admin;

namespace TemplateV2.Services.Managers.Contracts
{
    public interface IAdminManager
    {
        Task<CheckForAdminUserResponse> CheckForAdminUser();
    }
}
