using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Razor.Authorization.Requirements;

namespace TemplateV2.Razor.Authorization.Handlers
{
    public class CreateAdminUserHandler : AuthorizationHandler<CreateAdminUserRequirement>
    {
        private readonly ICacheProvider _cacheProvider;

        public CreateAdminUserHandler(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateAdminUserRequirement requirement)
        {
            _cacheProvider.TryGet(CacheConstants.AdminUserExists, out bool? requiresAdminUser);
            if (requiresAdminUser.HasValue &&
                requiresAdminUser.Value == true)
            {
                context.Succeed(requirement);
            }
        }
    }
}
