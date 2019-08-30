using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Common.Notifications;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Models.ServiceModels.Admin.Roles;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Services.Admin
{
    public class RoleService : IRoleService
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

        public RoleService(
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

        public async Task<GetRolesResponse> GetRoles()
        {
            var response = new GetRolesResponse
            {
                Roles = await _cache.Roles()
            };

            return response;
        }

        public async Task<EnableRoleResponse> EnableRole(EnableRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new EnableRoleResponse();

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(u => u.Id == request.Id);

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.EnableRole(new Repositories.DatabaseRepos.UserRepo.Models.EnableRoleRequest()
                {
                    Id = role.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Roles);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleEnabled,
                Info = new Dictionary<string, string>()
                {
                    { "Role_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"Role '{role.Name}' has been enabled", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<DisableRoleResponse> DisableRole(DisableRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new DisableRoleResponse();

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(u => u.Id == request.Id);

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.DisableRole(new Repositories.DatabaseRepos.UserRepo.Models.DisableRoleRequest()
                {
                    Id = role.Id,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Roles);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleDisabled,
                Info = new Dictionary<string, string>()
                {
                    { "Role_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"Role '{role.Name}' has been disabled", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<GetRoleResponse> GetRole(GetRoleRequest request)
        {
            var response = new GetRoleResponse();

            var roles = await _cache.Roles();
            var permissions = await _cache.Permissions();
            var rolePermissions = await _cache.RolePermissions();

            var role = roles.FirstOrDefault(r => r.Id == request.Id);
            if (role == null)
            {
                response.Notifications.AddError($"Could not find role with Id {request.Id}");
                return response;
            }

            var rolesPermissions = rolePermissions.Where(rc => rc.Role_Id == request.Id).Select(rc => rc.Permission_Id);

            response.Role = role;
            response.Permissions = permissions.Where(c => rolesPermissions.Contains(c.Id)).ToList();

            response.Role = role;
            return response;
        }

        public async Task<CreateRoleResponse> CreateRole(CreateRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new CreateRoleResponse();

            var duplicateResponse = await _accountService.DuplicateRoleCheck(new DuplicateRoleCheckRequest()
            {
                Name = request.Name
            });

            if (duplicateResponse.Notifications.HasErrors)
            {
                response.Notifications.Add(duplicateResponse.Notifications);
                return response;
            }

            int id;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                id = await uow.UserRepo.CreateRole(new Repositories.DatabaseRepos.UserRepo.Models.CreateRoleRequest()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Created_By = sessionUser.Id,
                });
                uow.Commit();
            }

            _cache.Remove(CacheConstants.Roles);

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(r => r.Id == id);

            var selectedPermissionIds = request.PermissionIds.Where(p => p.Selected).Select(p => p.Id).ToList();
            await CreateOrDeleteRolePermissions(selectedPermissionIds, id, sessionUser.Id);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleCreated,
                Info = new Dictionary<string, string>()
                {
                    { "Role Name", request.Name.ToString() }
                }
            });

            response.Notifications.Add($"Role '{request.Name}' has been created", NotificationTypeEnum.Success);
            return response;
        }

        public async Task<UpdateRoleResponse> UpdateRole(UpdateRoleRequest request)
        {
            var sessionUser = await _sessionManager.GetUser();
            var response = new UpdateRoleResponse();

            var roles = await _cache.Roles();
            var role = roles.FirstOrDefault(u => u.Id == request.Id);

            using (var uow = _uowFactory.GetUnitOfWork())
            {
                await uow.UserRepo.UpdateRole(new Repositories.DatabaseRepos.UserRepo.Models.UpdateRoleRequest()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Updated_By = sessionUser.Id
                });
                uow.Commit();
            }

            var selectedPermissionIds = request.PermissionIds.Where(p => p.Selected).Select(p => p.Id).ToList();
            await CreateOrDeleteRolePermissions(selectedPermissionIds, request.Id, sessionUser.Id);

            _cache.Remove(CacheConstants.Roles);
            _cache.Remove(CacheConstants.RolePermissions);

            await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
            {
                EventKey = SessionEventKeys.RoleUpdated,
                Info = new Dictionary<string, string>()
                {
                    { "Role_Id", request.Id.ToString() }
                }
            });

            response.Notifications.Add($"Role '{role.Name}' has been updated", NotificationTypeEnum.Success);
            return response;
        }

        private async Task CreateOrDeleteRolePermissions(List<int> newPermissions, int roleId, int loggedInUserId)
        {
            var rolePermissions = await _cache.RolePermissions();
            var existingPermissions = rolePermissions.Where(rc => rc.Role_Id == roleId).Select(rc => rc.Permission_Id);

            var permissionsToBeDeleted = existingPermissions.Where(ur => !newPermissions.Contains(ur));
            var permissionsToBeCreated = newPermissions.Where(ur => !existingPermissions.Contains(ur));

            if (permissionsToBeDeleted.Any() || permissionsToBeCreated.Any())
            {
                using (var uow = _uowFactory.GetUnitOfWork())
                {
                    foreach (var permissionId in permissionsToBeCreated)
                    {
                        await uow.UserRepo.CreateRolePermission(new Repositories.DatabaseRepos.UserRepo.Models.CreateRolePermissionRequest()
                        {
                            Role_Id = roleId,
                            Permission_Id = permissionId,
                            Created_By = loggedInUserId
                        });

                    }

                    foreach (var permissionId in permissionsToBeDeleted)
                    {
                        await uow.UserRepo.DeleteRolePermission(new Repositories.DatabaseRepos.UserRepo.Models.DisableRolePermissionRequest()
                        {
                            Role_Id = roleId,
                            Permission_Id = permissionId,
                            Updated_By = loggedInUserId
                        });

                    }
                    uow.Commit();
                }
                _cache.Remove(CacheConstants.RolePermissions);

                await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                {
                    EventKey = SessionEventKeys.RolePermissionsUpdated,
                    Info = new Dictionary<string, string>()
                    {
                       { "Role_Id", roleId.ToString() }
                    }
                });
            }
        }

        #endregion
    }
}
