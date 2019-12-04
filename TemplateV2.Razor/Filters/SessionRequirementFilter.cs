using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Filters
{
    /// <summary>
    /// this class ensures that an authenticated user has a session when they are logging in (in-case the application state is restarted)
    /// </summary>
    public class SessionRequirementFilter : IAsyncAuthorizationFilter
    {
        private readonly ISessionManager _sessionManager;

        public SessionRequirementFilter(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Path.HasValue &&
                context.HttpContext.Request.Path.Value.Contains("/Diagnostics/"))
            {

            }
            else
            {
                var session = await _sessionManager.GetSession();
                if (context.HttpContext.User.Identity.IsAuthenticated && !session.SessionEntity.User_Id.HasValue)
                {
                    await context.HttpContext.SignOutAsync();
                    context.Result = new ChallengeResult();
                }
            }
        }
    }
}
