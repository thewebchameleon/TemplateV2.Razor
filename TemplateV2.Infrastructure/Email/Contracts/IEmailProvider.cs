using System.Threading.Tasks;
using TemplateV2.Infrastructure.Email.Models;

namespace TemplateV2.Infrastructure.Email.Contracts
{
    public interface IEmailProvider
    {
        Task Send(SendRequest request);
    }
}
