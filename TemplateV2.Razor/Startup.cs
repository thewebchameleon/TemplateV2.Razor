using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using TemplateV2.Services.Admin;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add our config options
            services.Configure<ConnectionStringSettings>(_configuration.GetSection("ConnectionStrings"));
            services.Configure<CacheSettings>(_configuration.GetSection("Cache"));
            services.Configure<EmailSettings>(_configuration.GetSection("Email"));

            // add our config options directly to dependancy injection
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<ConnectionStringSettings>>().Value);
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<CacheSettings>>().Value);
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<EmailSettings>>().Value);

            // Configure services
            services.AddTransient<ISessionProvider, SessionProvider>();
            services.AddTransient<IEmailProvider, EmailProvider>();

            services.AddDistributedMemoryCache();
            services.AddTransient<ICacheProvider, MemoryCacheProvider>();

            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IHomeService, HomeService>();

            //admin
            services.AddTransient<IConfigurationService, ConfigurationService>();
            services.AddTransient<IPermissionsService, PermissionsService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISessionService, SessionService>();

            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<ICacheManager, CacheManager>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<IEmailManager, EmailManager>();

            services.AddTransient<IEmailTemplateRepo, EmailTemplateRepo>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddHttpClient();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;

                // Although this setting breaks OAuth2 and other cross-origin authentication schemes, 
                // it elevates the level of cookie security for other types of apps that don't rely 
                // on cross-origin request processing.
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            // authentication and authorization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(ApplicationConstants.SessionTimeoutSeconds);
                options.SlidingExpiration = true;

                options.Cookie = ApplicationConstants.SecureNamelessCookie;
                options.Cookie.Name = "Authentication";
            });
            //services.AddAuthorization(options => { });

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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SessionRequirementFilter));
                options.Filters.Add(typeof(SessionLoggingFilter));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/Index");
                options.Conventions.AuthorizePage("/Account/Profile");
                options.Conventions.AuthorizeFolder("/Admin");
                options.Conventions.AllowAnonymousToPage("/Account/ResetPasswsord");
            });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(ApplicationConstants.SupportedCultures.First().Name);
                options.SupportedCultures = ApplicationConstants.SupportedCultures;
                options.SupportedUICultures = ApplicationConstants.SupportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseSessionMiddleware();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseMvc();
        }
    }
}
