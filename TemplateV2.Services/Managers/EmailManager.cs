using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TemplateV2.Common.Extensions;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Email.Contracts;
using TemplateV2.Models;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.EmailTemplates;
using TemplateV2.Models.ServiceModels.Email;
using TemplateV2.Repositories.ServiceRepos.EmailTemplateRepo.Contracts;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Services.Managers
{
    public class EmailManager : IEmailManager
    {
        #region Instance Fields

        private readonly IEmailProvider _emailProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheManager _cache;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IEmailTemplateRepo _emailTemplateRepo;

        #endregion

        #region Constructors

        public EmailManager(
            IEmailProvider emailProvider,
            IHttpContextAccessor httpContextAccessor,
            ICacheManager cache,
            IUnitOfWorkFactory uowFactory,
            IEmailTemplateRepo emailTemplateRepo)
        {
            _emailProvider = emailProvider;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _uowFactory = uowFactory;
            _emailTemplateRepo = emailTemplateRepo;
        }

        #endregion

        #region Public Methods

        public async Task SendAccountActivation(SendAccountActivationRequest request)
        {
            var activationToken = string.Empty;

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.UserId
                });

                activationToken = GenerateUniqueUserToken(uow);

                await uow.UserRepo.CreateUserToken(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserTokenRequest()
                {
                    User_Id = request.UserId,
                    Token = new Guid(activationToken),
                    Type_Id = (int)TokenTypeEnum.AccountActivation,
                    Created_By = ApplicationConstants.SystemUserId,
                });
                uow.Commit();
            }

            var configuration = await _cache.Configuration();
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();

            var templateHtml = await _emailTemplateRepo.GetAccountActivationHTML();
            var template = new AccountActivationTemplate(templateHtml, baseUrl)
            {
                ActivationUrl = $"{baseUrl}/Account/ActivateAccount?token={activationToken}"
            };

            await _emailProvider.Send(new Infrastructure.Email.Models.SendRequest()
            {
                FromAddress = configuration.System_From_Email_Address,
                ToAddress = user.Email_Address,
                Subject = template.Subject,
                Body = template.GetHTMLContent()
            });
        }

        public async Task SendResetPassword(SendResetPasswordRequest request)
        {
            var resetToken = string.Empty;

            UserEntity user;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                user = await uow.UserRepo.GetUserById(new Repositories.DatabaseRepos.UserRepo.Models.GetUserByIdRequest()
                {
                    Id = request.UserId
                });

                resetToken = GenerateUniqueUserToken(uow);

                await uow.UserRepo.CreateUserToken(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserTokenRequest()
                {
                    User_Id = request.UserId,
                    Token = new Guid(resetToken),
                    Type_Id = (int)TokenTypeEnum.ResetPassword,
                    Created_By = ApplicationConstants.SystemUserId,
                });
                uow.Commit();
            }

            var configuration = await _cache.Configuration();
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();

            var templateHtml = await _emailTemplateRepo.GetResetPasswordHTML();
            var template = new ResetPasswordTemplate(templateHtml, baseUrl)
            {
                ResetPasswordUrl = $"{baseUrl}/Account/ResetPassword?token={resetToken}"
            };

            await _emailProvider.Send(new Infrastructure.Email.Models.SendRequest()
            {
                FromAddress = configuration.System_From_Email_Address,
                ToAddress = user.Email_Address,
                Subject = template.Subject,
                Body = template.GetHTMLContent()
            });
        }

        public async Task SendFeedback(SendFeedbackRequest request)
        {
            var configuration = await _cache.Configuration();
            var baseUrl = _httpContextAccessor.HttpContext.Request.GetBaseUrl();

            var templateHtml = await _emailTemplateRepo.GetSendFeedbackHTML();
            var template = new SendFeedbackTemplate(templateHtml, baseUrl)
            {
                Name = request.Name,
                EmailAddress = request.EmailAddress,
                Message = request.Message
            };

            await _emailProvider.Send(new Infrastructure.Email.Models.SendRequest()
            {
                FromAddress = configuration.System_From_Email_Address,
                ToAddress = configuration.Contact_Email_Address,
                Subject = template.Subject,
                Body = template.GetHTMLContent()
            });
        }

        #endregion

        #region Private Methods

        private string GenerateUniqueUserToken(IUnitOfWork uow)
        {
            var generatedCode = GenerateGuid();

            while (CheckUserTokenExists(uow, generatedCode))
            {
                generatedCode = GenerateGuid();
            }
            return generatedCode;
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        private bool CheckUserTokenExists(IUnitOfWork uow, string token)
        {
            var tokenResult = uow.UserRepo.GetUserTokenByGuid(new Repositories.DatabaseRepos.UserRepo.Models.GetUserTokenByGuidRequest()
            {
                Guid = new Guid(token)
            });
            tokenResult.Wait();
            return tokenResult.Result != null;
        }

        #endregion
    }
}
