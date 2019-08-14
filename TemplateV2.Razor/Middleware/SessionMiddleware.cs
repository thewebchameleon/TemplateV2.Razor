using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISessionManager sessionManager)
        {
            // calling this method ensures a session is created if one does not already exist.
            var session = await sessionManager.GetSession();

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
