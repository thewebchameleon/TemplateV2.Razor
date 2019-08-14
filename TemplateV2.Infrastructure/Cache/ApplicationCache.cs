using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Infrastructure.Repositories.UnitOfWork.Contracts;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Infrastructure.Cache
{
    public class ApplicationCache : IApplicationCache
    {
        #region Instance Fields

        private readonly ICacheProvider _cacheProvider;
        private readonly IUnitOfWorkFactory _uowFactory;

        #endregion

        #region Constructor

        public ApplicationCache(IUnitOfWorkFactory uowFactory, ICacheProvider cacheProvider)
        {
            _uowFactory = uowFactory;
            _cacheProvider = cacheProvider;
        }

        #endregion

        #region Public Methods

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

        public void Remove(string key)
        {
            _cacheProvider.Remove(key);
        }

        #endregion
    }
}
