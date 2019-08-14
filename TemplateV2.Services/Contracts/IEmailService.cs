using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Email;

namespace TemplateV2.Services.Contracts
{
    public interface IEmailService
    {
        Task SendAccountActivation(SendAccountActivationRequest request);

        Task SendResetPassword(SendResetPasswordRequest request);

        Task SendForgotPassword(SendForgotPasswordRequest request);
    }
}
