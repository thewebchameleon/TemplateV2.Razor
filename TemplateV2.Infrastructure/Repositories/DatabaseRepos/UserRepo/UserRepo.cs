using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Adapters;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Contracts;
using TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo
{
    public class UserRepo : BaseSQLRepo, IUserRepo
    {
        #region Instance Fields

        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        #endregion

        #region Constructor

        public UserRepo(IDbConnection connection, IDbTransaction transaction, ConnectionStringSettings connectionStrings)
            : base(connectionStrings)
        {
            _connection = connection;
            _transaction = transaction;
        }

        #endregion

        #region Public Methods

        public async Task ActivateAccount(ActivateAccountRequest request)
        {
            var sqlStoredProc = "sp_user_activate";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been updated");
            }
        }

        public async Task CreateUserToken(CreateUserTokenRequest request)
        {
            var sqlStoredProc = "sp_user_token_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new System.Exception("No items have been created");
            }
        }

        public async Task<UserEntity> FetchDuplicateUser(FetchDuplicateUserRequest request)
        {
            var sqlStoredProc = "sp_user_duplicate_check";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<List<UserEntity>> GetUsers()
        {
            var sqlStoredProc = "sp_users_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<RoleEntity>> GetRoles()
        {
            var sqlStoredProc = "sp_roles_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<RoleEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<RolePermissionEntity>> GetRolePermissions()
        {
            var sqlStoredProc = "sp_role_permissions_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<RolePermissionEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<UserTokenEntity> GetUserTokenByGuid(GetUserTokenByGuidRequest request)
        {
            var sqlStoredProc = "sp_user_token_get_by_guid";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserTokenEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<List<UserRoleEntity>> GetUserRoles()
        {
            var sqlStoredProc = "sp_user_roles_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserRoleEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<int> CreateRole(CreateRoleRequest request)
        {
            var sqlStoredProc = "sp_role_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items were created");
            }
            return response.FirstOrDefault();
        }

        public async Task UpdateRole(UpdateRoleRequest request)
        {
            var sqlStoredProc = "sp_role_update";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been updated");
            }
        }

        public async Task DisableRole(DisableRoleRequest request)
        {
            var sqlStoredProc = "sp_role_disable";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been disabled");
            }
        }

        public async Task<int> CreateUser(CreateUserRequest request)
        {
            var sqlStoredProc = "sp_user_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been created");
            }
            return response.FirstOrDefault();
        }

        public async Task DisableUser(DisableUserRequest request)
        {
            var sqlStoredProc = "sp_user_disable";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been disabled");
            }
        }

        public async Task UpdateUser(UpdateUserRequest request)
        {
            var sqlStoredProc = "sp_user_update";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been updated");
            }
        }

        public async Task<int> CreateUserRole(CreateUserRoleRequest request)
        {
            var sqlStoredProc = "sp_user_role_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items were created");
            }
            return response.First();
        }

        public async Task DeleteUserRole(DeleteUserRoleRequest request)
        {
            var sqlStoredProc = "sp_user_role_delete";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items were deleted");
            }
        }

        public async Task<int> CreateRolePermission(CreateRolePermissionRequest request)
        {
            var sqlStoredProc = "sp_role_permission_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been created");
            }
            return response.First();
        }

        public async Task DeleteRolePermission(DisableRolePermissionRequest request)
        {
            var sqlStoredProc = "sp_role_permission_delete";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been deleted");
            }
        }

        public async Task<List<PermissionEntity>> GetPermissions()
        {
            var sqlStoredProc = "sp_permissions_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<PermissionEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<UserEntity> GetUserById(GetUserByIdRequest request)
        {
            var sqlStoredProc = "sp_user_get_by_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<UserEntity> GetUserByUsername(GetUserByUsernameRequest request)
        {
            var sqlStoredProc = "sp_user_get_by_username";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<UserEntity> GetUserByEmail(GetUserByEmailRequest request)
        {
            var sqlStoredProc = "sp_user_get_by_email";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<UserEntity> GetUserByMobileNumber(GetUserByMobileNumberRequest request)
        {
            var sqlStoredProc = "sp_user_get_by_mobile_number";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task EnableRole(EnableRoleRequest request)
        {
            var sqlStoredProc = "sp_role_enable";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been enabled");
            }
        }

        public async Task EnableUser(EnableUserRequest request)
        {
            var sqlStoredProc = "sp_user_enable";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been enabled");
            }
        }

        public async Task UpdatePermission(UpdatePermissionRequest request)
        {
            var sqlStoredProc = "sp_permission_update";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items have been updated");
            }
        }

        public async Task<int> CreatePermission(CreatePermissionRequest request)
        {
            var sqlStoredProc = "sp_permission_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items have been created");
            }
            return response.FirstOrDefault();
        }

        public async Task LockoutUser(LockoutUserRequest request)
        {
            var sqlStoredProc = "sp_user_lockout";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been locked");
            }
        }

        public async Task UnlockUser(UnlockUserRequest request)
        {
            var sqlStoredProc = "sp_user_unlock";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.First() == 0)
            {
                throw new Exception("No items have been unlocked");
            }
        }

        public async Task AddInvalidLoginAttempt(AddInvalidLoginAttemptRequest request)
        {
            var sqlStoredProc = "sp_user_add_invalid_login_attempt";

            await DapperAdapter.GetFromStoredProcAsync<int?>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

        }

        public async Task UpdateUserPassword(UpdateUserPasswordRequest request)
        {
            var sqlStoredProc = "sp_user_update_password";

            await DapperAdapter.GetFromStoredProcAsync<int?>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);
        }

        public async Task ProcessUserToken(ProcessUserTokenRequest request)
        {
            var sqlStoredProc = "sp_user_token_process";

            await DapperAdapter.GetFromStoredProcAsync<int?>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);
        }

        public async Task<List<UserPermissionEntity>> GetUserPermissionsByUserId(GetUserPermissionsByIdRequest request)
        {
            var sqlStoredProc = "sp_user_permissions_get_by_user_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserPermissionEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<UserRoleEntity>> GetUserRolesByUserId(GetUserRolesByUserIdRequest request)
        {
            var sqlStoredProc = "sp_user_roles_get_by_user_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<UserRoleEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        #endregion
    }
}
