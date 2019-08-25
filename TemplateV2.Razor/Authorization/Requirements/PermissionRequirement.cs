using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TemplateV2.Razor.Authorization.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public List<string> Permissions { get; }

        public PermissionRequirement(string permission)
        {
            Permissions = new List<string>();
            Permissions.Add(permission);
        }

        public PermissionRequirement(List<string> permissions)
        {
            Permissions = permissions;
        }
    }
}
