using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Home;

namespace TemplateV2.Services.Contracts
{
    public interface IHomeService
    {
        Task<GetHomeResponse> GetHome();
    }
}
