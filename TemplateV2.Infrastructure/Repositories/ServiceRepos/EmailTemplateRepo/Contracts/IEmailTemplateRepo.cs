using System.Threading.Tasks;

namespace TemplateV2.Infrastructure.Repositories.ServiceRepos.EmailTemplateRepo.Contracts
{
    public interface IEmailTemplateRepo
    {
        Task<string> GetForgotPasswordHTML();

        Task<string> GetResetPasswordHTML();

        Task<string> GetAccountActivationHTML();

        Task<string> GetContactMessageHTML();
    }
}