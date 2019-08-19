using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Users;

namespace TemplateV2.Services.Admin.Contracts
{
    public interface IUserService
    {
        Task<GetUsersResponse> GetUsers();

        Task<DisableUserResponse> DisableUser(DisableUserRequest request);

        Task<EnableUserResponse> EnableUser(EnableUserRequest request);

        Task<UnlockUserResponse> UnlockUser(UnlockUserRequest request);

        Task<GetUserResponse> GetUser(GetUserRequest request);

        Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);

        Task<CreateUserResponse> CreateUser(CreateUserRequest request);

        Task<ConfirmRegistrationResponse> ConfirmRegistration(ConfirmRegistrationRequest request);

        Task<GenerateResetPasswordUrlResponse> GenerateResetPasswordUrl(GenerateResetPasswordUrlRequest request);

        Task<SendResetPasswordEmailResponse> SendResetPasswordEmail(SendResetPasswordEmailRequest request);
    }
}
