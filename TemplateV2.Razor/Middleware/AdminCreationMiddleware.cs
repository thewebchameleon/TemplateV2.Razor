using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Middleware
{
    public class AdminCreationMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminCreationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAdminManager adminManager)
        {
            // check if an admin user exists
            var response = await adminManager.CheckForAdminUser();
            if (!response.AdminUserExists && context.Request.Path != "/Admin/CreateAdminUser")
            {
                context.Response.Redirect("/Admin/CreateAdminUser");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
