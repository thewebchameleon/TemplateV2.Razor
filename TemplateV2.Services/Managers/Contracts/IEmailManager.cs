using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Email;

namespace TemplateV2.Services.Managers.Contracts
{
    public interface IEmailManager
    {
        Task SendAccountActivation(SendAccountActivationRequest request);

        Task SendResetPassword(SendResetPasswordRequest request);

        Task SendForgotPassword(SendForgotPasswordRequest request);
    }
}
