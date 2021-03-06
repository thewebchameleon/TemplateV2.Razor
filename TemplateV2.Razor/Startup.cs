using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Infrastructure.Email;
using TemplateV2.Infrastructure.Email.Contracts;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Infrastructure.Session.Contracts;
using TemplateV2.Repositories.UnitOfWork;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Razor.Filters;
using TemplateV2.Razor.Middleware;
using TemplateV2.Services;
using TemplateV2.Services.Contracts;
using TemplateV2.Repositories.ServiceRepos.EmailTemplateRepo.Contracts;
using TemplateV2.Repositories.ServiceRepos.EmailTemplateRepo;
using TemplateV2.Services.Managers.Contracts;
using TemplateV2.Services.Managers;
using System.Globalization;
using TemplateV2.Services.Admin;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Razor.Authorization;
using TemplateV2.Razor.Authorization.Requirements;
using TemplateV2.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using TemplateV2.Razor.Authorization.Handlers;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Hosting;

namespace TemplateV2.Razor
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Configuration

            // Add our appsettings.json config options
            services.Configure<ConnectionStringSettings>(_configuration.GetSection("ConnectionStrings"));
            services.Configure<CacheSettings>(_configuration.GetSection("Cache"));
            services.Configure<EmailSettings>(_configuration.GetSection("Email"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // Check whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;

                // Although this setting breaks OAuth2 and other cross-origin authentication schemes, 
                // it elevates the level of cookie security for other types of apps that don't rely 
                // on cross-origin request processing.
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            });

            #endregion

            #region Services

            // Infrastructure Services
            services.AddTransient<ISessionProvider, SessionProvider>();
            services.AddTransient<IEmailProvider, SendGridEmailProvider>();

            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            services.AddApplicationInsightsTelemetry();

            // Caching
            services.AddDistributedMemoryCache();
            services.AddTransient<ICacheProvider, MemoryCacheProvider>();

            // Business Logic Services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IDashboardService, DashboardService>();

            // Business Logic Admin Service
            services.AddTransient<IConfigurationService, ConfigurationService>();
            services.AddTransient<IPermissionsService, PermissionsService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IAdminService, AdminService>();

            // Business Logic Managers
            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<ICacheManager, CacheManager>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<IEmailManager, EmailManager>();
            services.AddTransient<IAdminManager, AdminManager>();

            // Business Logic Service Repos
            services.AddTransient<IEmailTemplateRepo, EmailTemplateRepo>();

            #endregion

            #region Authentication and Authorization

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Error/401";
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(ApplicationConstants.SessionTimeoutSeconds);
                options.SlidingExpiration = true;

                options.Cookie = ApplicationConstants.SecureNamelessCookie;
                options.Cookie.Name = "Authentication";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyConstants.ViewSessions, policy => policy.Requirements.Add(new PermissionRequirement(PermissionKeys.ViewSessions)));
                options.AddPolicy(PolicyConstants.ManageUsers, policy => policy.Requirements.Add(new PermissionRequirement(PermissionKeys.ManageUsers)));
                options.AddPolicy(PolicyConstants.ManageRoles, policy => policy.Requirements.Add(new PermissionRequirement(PermissionKeys.ManageRoles)));
                options.AddPolicy(PolicyConstants.ManageConfiguration, policy => policy.Requirements.Add(new PermissionRequirement(PermissionKeys.ManageConfiguration)));

                options.AddPolicy(PolicyConstants.CreateAdminUser, policy => policy.Requirements.Add(new CreateAdminUserRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, PermissionsHandler>();

            #endregion

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(ApplicationConstants.SessionTimeoutSeconds);

                options.Cookie = ApplicationConstants.SecureNamelessCookie;
                options.Cookie.Name = "Session";
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
                options.EnableForHttps = true; // https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression#compression-with-secure-protocol
            });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SessionLoggingFilter));
                options.Filters.Add(typeof(OpenGraphPageFilter));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddRazorPagesOptions(options =>
            {
                // apply authorization by default to all pages
                options.Conventions.AuthorizeFolder("/");

                // white-listed routes
                options.Conventions.AllowAnonymousToFolder("/Error");
                options.Conventions.AllowAnonymousToFolder("/Email");

                options.Conventions.AllowAnonymousToPage("/Account/ForgotPassword");
                options.Conventions.AllowAnonymousToPage("/Account/Login");
                options.Conventions.AllowAnonymousToPage("/Account/Register");
                options.Conventions.AllowAnonymousToPage("/Account/ResetPassword");

                // custom authorization
                options.Conventions.AuthorizeFolder("/Admin/Sessions", PolicyConstants.ViewSessions);
                options.Conventions.AuthorizeFolder("/Admin/Users", PolicyConstants.ManageUsers);
                options.Conventions.AuthorizeFolder("/Admin/Roles", PolicyConstants.ManageRoles);
                options.Conventions.AuthorizeFolder("/Admin/Configuration", PolicyConstants.ManageConfiguration);
                options.Conventions.AuthorizeFolder("/Admin/Permissions", PolicyConstants.ManageConfiguration);
                options.Conventions.AuthorizeFolder("/Admin/SessionEvents", PolicyConstants.ManageConfiguration);

                options.Conventions.AllowAnonymousToPage("/Admin/CreateAdminUser"); // remove default authorization
                options.Conventions.AuthorizePage("/Admin/CreateAdminUser", PolicyConstants.CreateAdminUser);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // force the application to run under the specified culture
            CultureInfo.DefaultThreadCurrentCulture = ApplicationConstants.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = ApplicationConstants.Culture;

            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + ApplicationConstants.StaticFileCachingSeconds;
                }
            });

            app.UseRouting();
            app.UseSession();
            app.UseAdminCreationMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
