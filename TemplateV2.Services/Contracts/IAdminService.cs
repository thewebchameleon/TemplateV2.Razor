using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Configuration;
using TemplateV2.Models.ServiceModels.Admin.Permissions;
using TemplateV2.Models.ServiceModels.Admin.Roles;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Models.ServiceModels.Admin.Sessions;
using TemplateV2.Models.ServiceModels.Admin.Users;

namespace TemplateV2.Services.Contracts
{
    public interface IAdminService
    {
        #region Users

        Task<GetUsersResponse> GetUsers();

        Task<DisableUserResponse> DisableUser(DisableUserRequest request);

        Task<EnableUserResponse> EnableUser(EnableUserRequest request);

        Task<UnlockUserResponse> UnlockUser(UnlockUserRequest request);

        Task<GetUserResponse> GetUser(GetUserRequest request);

        Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);

        Task<CreateUserResponse> CreateUser(CreateUserRequest request);

        Task<ConfirmRegistrationResponse> ConfirmRegistration(ConfirmRegistrationRequest request);

        Task<GenerateResetPasswordUrlResponse> GenerateResetPasswordUrl(GenerateResetPasswordUrlRequest request);

        Task<SendResetPasswordEmailResponse> SendResetPasswordEmail(SendResetPasswordEmailRequest request);

        #endregion

        #region Roles

        Task<GetRolesResponse> GetRoles();

        Task<DisableRoleResponse> DisableRole(DisableRoleRequest request);

        Task<EnableRoleResponse> EnableRole(EnableRoleRequest request);

        Task<GetRoleResponse> GetRole(GetRoleRequest request);

        Task<UpdateRoleResponse> UpdateRole(UpdateRoleRequest request);

        Task<CreateRoleResponse> CreateRole(CreateRoleRequest request);

        #endregion

        #region Permissions

        Task<GetPermissionsResponse> GetPermissions();

        Task<GetPermissionResponse> GetPermission(GetPermissionRequest request);

        Task<UpdatePermissionResponse> UpdatePermission(UpdatePermissionRequest request);

        Task<CreatePermissionResponse> CreatePermission(CreatePermissionRequest request);

        #endregion

        #region Configuration

        Task<GetConfigurationItemsResponse> GetConfigurationItems();

        Task<GetConfigurationItemResponse> GetConfigurationItem(GetConfigurationItemRequest request);

        Task<UpdateConfigurationItemResponse> UpdateConfigurationItem(UpdateConfigurationItemRequest request);

        Task<CreateConfigurationItemResponse> CreateConfigurationItem(CreateConfigurationItemRequest request);

        #endregion

        #region Sessions

        Task<GetSessionsResponse> GetSessions(GetSessionsRequest request);

        Task<GetSessionResponse> GetSession(GetSessionRequest request);

        Task<GetSessionLogsResponse> GetSessionLogs(GetSessionLogsRequest request);

        Task<GetSessionEventsResponse> GetSessionEvents();

        Task<GetSessionEventResponse> GetSessionEvent(GetSessionEventRequest request);

        Task<UpdateSessionEventResponse> UpdateSessionEvent(UpdateSessionEventRequest request);

        Task<CreateSessionEventResponse> CreateSessionEvent(CreateSessionEventRequest request);

        #endregion
    }
}
