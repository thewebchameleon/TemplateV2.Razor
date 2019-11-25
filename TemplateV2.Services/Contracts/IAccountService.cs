using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Models.ServiceModels.Account;

namespace TemplateV2.Services.Contracts
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);

        Task<RegisterResponse> Register(RegisterRequest request);

        Task<ActivateAccountResponse> ActivateAccount(ActivateAccountRequest request);

        Task<ValidateResetPasswordTokenResponse> ValidateResetPasswordToken(ValidateResetPasswordTokenRequest request);

        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);

        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);

        Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request);

        Task Logout();

        Task<GetProfileResponse> GetProfile();

        Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest request);

        Task<DuplicateUserCheckResponse> DuplicateUserCheck(DuplicateUserCheckRequest request);

        Task<DuplicateRoleCheckResponse> DuplicateRoleCheck(DuplicateRoleCheckRequest request);

        Task<GetActivityLogsResponse> GetActivityLogs(GetActivityLogsRequest request);

        Task<SendFeedbackResponse> SendFeedback(SendFeedbackRequest request);
    }
}
