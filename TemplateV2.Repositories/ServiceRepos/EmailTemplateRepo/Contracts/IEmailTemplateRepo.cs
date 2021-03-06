using System.Threading.Tasks;

namespace TemplateV2.Repositories.ServiceRepos.EmailTemplateRepo.Contracts
{
    public interface IEmailTemplateRepo
    {
        Task<string> GetResetPasswordHTML();

        Task<string> GetAccountActivationHTML();

        Task<string> GetSendFeedbackHTML();
    }
}