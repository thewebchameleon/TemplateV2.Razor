using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin;

namespace TemplateV2.Services.Admin.Contracts
{
    public interface IAdminService
    {
        Task<CheckIfCanCreateAdminUserResponse> CheckIfCanCreateAdminUser();

        Task<CreateAdminUserResponse> CreateAdminUser(CreateAdminUserRequest request);
    }
}
