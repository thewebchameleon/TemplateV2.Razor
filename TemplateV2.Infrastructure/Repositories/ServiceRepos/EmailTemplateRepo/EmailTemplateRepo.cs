using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using TemplateV2.Common.Extensions;
using TemplateV2.Infrastructure.HttpClients;
using TemplateV2.Infrastructure.Repositories.ServiceRepos.EmailTemplateRepo.Contracts;

namespace TemplateV2.Infrastructure.Repositories.ServiceRepos.EmailTemplateRepo
{
    public class EmailTemplateRepo : IEmailTemplateRepo
    {
        #region Instance Fields

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public EmailTemplateRepo(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Public Methods

        public async Task<string> GetForgotPasswordHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/email-template/forgot-password.html");
            return httpResponse.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> GetResetPasswordHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/email-template/reset-password.html");
            return httpResponse.ReadMessage<string>();
        }

        public async Task<string> GetAccountActivationHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/email-template/account-activation.html");
            return httpResponse.ReadMessage<string>();
        }

        public async Task<string> GetContactMessageHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/email-template/contact-message.html");
            return httpResponse.ReadMessage<string>();
        }

        #endregion
    }
}
