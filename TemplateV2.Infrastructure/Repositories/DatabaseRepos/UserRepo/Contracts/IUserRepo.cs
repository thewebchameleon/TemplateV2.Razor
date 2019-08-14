using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Contracts
{
    public interface IUserRepo
    {
        Task<int> CreateRole(CreateRoleRequest request);

        Task UpdateRole(UpdateRoleRequest request);

        Task DisableRole(DisableRoleRequest request);

        Task EnableRole(EnableRoleRequest request);

        Task<int> CreateUser(CreateUserRequest request);

        Task DisableUser(DisableUserRequest request);

        Task EnableUser(EnableUserRequest request);

        Task UpdateUser(UpdateUserRequest request);

        Task<int> CreateUserRole(CreateUserRoleRequest request);

        Task DeleteUserRole(DeleteUserRoleRequest request);

        Task ActivateAccount(ActivateAccountRequest request);

        Task CreateUserToken(CreateUserTokenRequest request);

        Task<UserEntity> FetchDuplicateUser(FetchDuplicateUserRequest request);

        Task<List<UserEntity>> GetUsers();

        Task<List<RoleEntity>> GetRoles();

        Task<List<RolePermissionEntity>> GetRolePermissions();

        Task<UserTokenEntity> GetUserTokenByGuid(GetUserTokenByGuidRequest request);

        Task UpdateUserPassword(UpdateUserPasswordRequest request);

        Task<List<UserRoleEntity>> GetUserRoles();

        Task<List<PermissionEntity>> GetPermissions();

        Task UpdatePermission(UpdatePermissionRequest request);

        Task<int> CreatePermission(CreatePermissionRequest request);

        Task<int> CreateRolePermission(CreateRolePermissionRequest request);

        Task DeleteRolePermission(DisableRolePermissionRequest request);

        Task<UserEntity> GetUserById(GetUserByIdRequest request);

        Task<UserEntity> GetUserByUsername(GetUserByUsernameRequest request);

        Task<UserEntity> GetUserByEmail(GetUserByEmailRequest request);

        Task<UserEntity> GetUserByMobileNumber(GetUserByMobileNumberRequest request);

        Task LockoutUser(LockoutUserRequest request);

        Task UnlockUser(UnlockUserRequest request);

        Task AddInvalidLoginAttempt(AddInvalidLoginAttemptRequest request);

        Task ProcessUserToken(ProcessUserTokenRequest request);

        Task<List<UserPermissionEntity>> GetUserPermissionsByUserId(GetUserPermissionsByIdRequest request);

        Task<List<UserRoleEntity>> GetUserRolesByUserId(GetUserRolesByUserIdRequest request);
    }
}
