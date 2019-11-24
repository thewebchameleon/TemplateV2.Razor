using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Managers.Contracts;
using System.Linq;
using TemplateV2.Infrastructure.Configuration;

namespace TemplateV2.Services.Managers
{
    public class CacheManager : ICacheManager
    {
        #region Instance Fields

        private readonly ICacheProvider _cacheProvider;
        private readonly IUnitOfWorkFactory _uowFactory;

        #endregion

        #region Constructor

        public CacheManager(
            ICacheProvider cacheProvider,
            IUnitOfWorkFactory uowFactory
        )
        {
            _cacheProvider = cacheProvider;
            _uowFactory = uowFactory;
        }

        #endregion

        #region Public Methods

        public void Remove(string key)
        {
            _cacheProvider.Remove(key);
        }

        public async Task<ApplicationConfiguration> Configuration()
        {
            var items = new List<ConfigurationEntity>();
            if (_cacheProvider.TryGet(CacheConstants.ConfigurationItems, out items))
            {
                return new ApplicationConfiguration(items);
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                items = await uow.ConfigurationRepo.GetConfigurationItems();
                uow.Commit();
            }

            _cacheProvider.Set(CacheConstants.ConfigurationItems, items);

            return new ApplicationConfiguration(items);
        }

        public async Task<ApplicationConfiguration_Javascript> Configuration_Javascript()
        {
            var items = new List<Client_Configuration>();
            if (_cacheProvider.TryGet(CacheConstants.ConfigurationItems_Javascript, out items))
            {
                return new ApplicationConfiguration_Javascript(items);
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var response = await uow.ConfigurationRepo.GetConfigurationItems();
                items = response.Where(i => i.Is_Client_Side).Select(i => new Client_Configuration()
                {
                    Key = i.Key,
                    Boolean_Value = i.Boolean_Value,
                    DateTime_Value = i.DateTime_Value,
                    Date_Value = i.Date_Value,
                    Decimal_Value = i.Decimal_Value,
                    Description = i.Description,
                    Int_Value = i.Int_Value,
                    Money_Value = i.Money_Value,
                    String_Value = i.String_Value,
                    Time_Value = i.Time_Value
                }).ToList();
                uow.Commit();
            }

            // add a 5 second buffer to the front-end session expiration
            var idleTimeout = ApplicationConstants.SessionTimeoutSeconds - 5;
            var modalTimeout = idleTimeout - (ApplicationConstants.SessionTimeoutSeconds - ApplicationConstants.SessionModalTimeoutSeconds);

            items.Add(new Client_Configuration()
            {
                Key = "IDLE_TIMEOUT_SECONDS",
                Int_Value = idleTimeout
            });

            items.Add(new Client_Configuration()
            {
                Key = "IDLE_TIMEOUT_MODAL_SECONDS",
                Int_Value = modalTimeout
            });

            _cacheProvider.Set(CacheConstants.ConfigurationItems_Javascript, items);

            return new ApplicationConfiguration_Javascript(items);
        }

        public async Task<List<PermissionEntity>> Permissions()
        {
            var items = new List<PermissionEntity>();
            if (_cacheProvider.TryGet(CacheConstants.Permissions, out items))
            {
                return items;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                items = await uow.UserRepo.GetPermissions();
                uow.Commit();
            }

            _cacheProvider.Set(CacheConstants.Permissions, items);

            return items;
        }

        public async Task<List<SessionEventEntity>> SessionEvents()
        {
            var items = new List<SessionEventEntity>();
            if (_cacheProvider.TryGet(CacheConstants.SessionEvents, out items))
            {
                return items;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                items = await uow.SessionRepo.GetSessionEvents();
                uow.Commit();
            }

            _cacheProvider.Set(CacheConstants.SessionEvents, items);

            return items;
        }

        public async Task<List<RolePermissionEntity>> RolePermissions()
        {
            var items = new List<RolePermissionEntity>();
            if (_cacheProvider.TryGet(CacheConstants.RolePermissions, out items))
            {
                return items;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                items = await uow.UserRepo.GetRolePermissions();
                uow.Commit();
            }

            _cacheProvider.Set(CacheConstants.RolePermissions, items);

            return items;
        }

        public async Task<List<RoleEntity>> Roles()
        {
            var items = new List<RoleEntity>();
            if (_cacheProvider.TryGet(CacheConstants.Roles, out items))
            {
                return items;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                items = await uow.UserRepo.GetRoles();

                uow.Commit();
            }
            _cacheProvider.Set(CacheConstants.Roles, items);

            return items;
        }

        public async Task<List<UserRoleEntity>> UserRoles()
        {
            var items = new List<UserRoleEntity>();
            if (_cacheProvider.TryGet(CacheConstants.UserRoles, out items))
            {
                return items;
            }

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                items = await uow.UserRepo.GetUserRoles();

                uow.Commit();
            }
            _cacheProvider.Set(CacheConstants.UserRoles, items);

            return items;
        }

        #endregion
    }
}
