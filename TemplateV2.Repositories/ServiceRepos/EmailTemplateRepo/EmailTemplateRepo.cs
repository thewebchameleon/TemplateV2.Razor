using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TemplateV2.Common.Extensions;
using TemplateV2.Infrastructure.HttpClients;
using TemplateV2.Repositories.ServiceRepos.EmailTemplateRepo.Contracts;

namespace TemplateV2.Repositories.ServiceRepos.EmailTemplateRepo
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
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/Email/ForgotPassword");

            var unprocessedHtml = httpResponse.Content.ReadAsStringAsync().Result;
            var processedHtml = PreMailer.Net.PreMailer.MoveCssInline(new Uri(baseUrl), unprocessedHtml);
            
            return processedHtml.Html;
        }

        public async Task<string> GetResetPasswordHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/Email/ResetPassword");

            var unprocessedHtml = httpResponse.Content.ReadAsStringAsync().Result;
            var processedHtml = PreMailer.Net.PreMailer.MoveCssInline(new Uri(baseUrl), unprocessedHtml);

            return processedHtml.Html;
        }

        public async Task<string> GetAccountActivationHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/Email/AccountActivation");

            var unprocessedHtml = httpResponse.Content.ReadAsStringAsync().Result;
            var processedHtml = PreMailer.Net.PreMailer.MoveCssInline(new Uri(baseUrl), unprocessedHtml);

            return processedHtml.Html;
        }

        public async Task<string> GetContactMessageHTML()
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();
            var httpResponse = await HttpHelper.Get(_httpClientFactory, $"{baseUrl}/Email/ContactMessage");

            var unprocessedHtml = httpResponse.Content.ReadAsStringAsync().Result;
            var processedHtml = PreMailer.Net.PreMailer.MoveCssInline(new Uri(baseUrl), unprocessedHtml);

            return processedHtml.Html;
        }

        #endregion
    }
}
