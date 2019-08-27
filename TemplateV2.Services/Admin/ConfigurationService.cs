using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Common.Notifications;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Models.ServiceModels.Admin.Configuration;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Services.Admin
{
    public class ConfigurationService : IConfigurationService
    {
        #region Instance Fields

        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;
        private readonly IEmailManager _emailManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly ICacheManager _cache;

        #endregion

        #region Constructor

        public ConfigurationService(
            IAccountService accountService,
            ISessionManager sessionManager,
            IEmailManager emailManager,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWorkFactory uowFactory,
            ICacheManager cache)
        {
            _uowFactory = uowFactory;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _accountService = accountService;
            _sessionManager = sessionManager;
            _emailManager = emailManager;
        }

        #endregion

        #region Public Methods

        public async Task<GetConfigurationItemsResponse> GetConfigurationItems()
        {
            var response = new GetConfigurationItemsResponse();

            var configuration = await _cache.Configuration();
            response.ConfigurationItems = configuration.Items;

            return response;
        }

        public async Task<GetConfigurationItemResponse> GetConfigurationItem(GetConfigurationItemRequest request)
        {
            var response = new GetConfigurationItemResponse();

            var configuration = await _cache.Configuration();
            var configItem = configuration.Items.FirstOrDefault(c => c.Id == request.Id);

            response.ConfigurationItem = configItem;

            return response;
        }

        public async Task<UpdateConfigurationItemResponse> UpdateConfigurationItem(UpdateConfigurationItemRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdateConfigurationItemResponse();

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.ConfigurationRepo.UpdateConfigurationItem(new Repositories.DatabaseRepos.ConfigurationRepo.Models.UpdateConfigurationItemRequest()
                {
                    Id = request.Id,
                    Description = request.Description,
                    Is_Client_Side = request.IsClientSide,
                    Boolean_Value = request.BooleanValue,
                    DateTime_Value = request.DateTimeValue,
                    Date_Value = request.DateValue,
                    Time_Value = request.TimeValue,
                    Decimal_Value = request.DecimalValue,
                    Int_Value = request.IntValue,
                    Money_Value = request.MoneyValue,
                    String_Value = request.StringValue,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.ConfigurationItems);

            var configuration = await _cache.Configuration();
            var configItem = configuration.Items.FirstOrDefault(c => c.Id == request.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.ConfigurationUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", configItem.Key }
                }
            });

            response.Notifications.Add($"Configuration item '{configItem.Key}' has been updated", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<CreateConfigurationItemResponse> CreateConfigurationItem(CreateConfigurationItemRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreateConfigurationItemResponse();

            var configuration = await _cache.Configuration();
            var configItem = configuration.Items.FirstOrDefault(c => c.Key == request.Key);

            if (configItem != null)
            {
                response.Notifications.AddError($"A configuration item already exists with the key {request.Key}");
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.ConfigurationRepo.CreateConfigurationItem(new Repositories.DatabaseRepos.ConfigurationRepo.Models.CreateConfigurationItemRequest()
                {
                    Key = request.Key,
                    Description = request.Description,
                    Is_Client_Side = request.IsClientSide,
                    Boolean_Value = request.BooleanValue,
                    DateTime_Value = request.DateTimeValue,
                    Date_Value = request.DateValue,
                    Time_Value = request.TimeValue,
                    Decimal_Value = request.DecimalValue,
                    Int_Value = request.IntValue,
                    Money_Value = request.MoneyValue,
                    String_Value = request.StringValue,
                    Created_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.ConfigurationItems);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.ConfigurationCreated,
                Info = new Dictionary<string, string>()
                {
                    { "Key", request.Key }
                }
            });

            response.Notifications.Add($"Configuration item '{request.Key}' has been created", NotificationTypeEnum.Success);
            return response;
        }

        #endregion

    }
}
