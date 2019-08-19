namespace TemplateV2.Infrastructure.Authorization
{
    /// <summary>
    /// These constants map to the [Key] field of each session event item
    /// </summary>
    public class PermissionKeys
    {
        public const string ViewAdmin = "ADMIN_VIEW";
        public const string ViewSessions = "SESSIONS_VIEW";
        public const string ManageUsers = "USERS_MANAGE";
        public const string ManageRoles = "ROLES_MANAGE";
        public const string ManagePermissions = "PERMISSIONS_MANAGE";
        public const string ManageSessionEvents = "SESSION_EVENTS_MANAGE";
        public const string ManageConfiguration = "CONFIGURATION_MANAGE";
    }
}
