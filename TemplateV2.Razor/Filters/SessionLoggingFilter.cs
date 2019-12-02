using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Common.Helpers;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Infrastructure.Session.Contracts;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using System.Collections.Generic;
using TemplateV2.Services.Managers.Contracts;
using System.Timers;
using System.Diagnostics;

namespace TemplateV2.Razor.Filters
{
    public class SessionLoggingFilter : IAsyncPageFilter
    {
        private readonly ISessionManager _sessionManager;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly ICacheManager _cache;
        private readonly ISessionProvider _sessionProvider;
        private readonly Stopwatch _stopwatch;

        public SessionLoggingFilter(
            ISessionManager sessionManager,
            ICacheManager cache,
            IUnitOfWorkFactory uowFactory,
            ISessionProvider sessionProvider)
        {
            _sessionManager = sessionManager;
            _uowFactory = uowFactory;
            _cache = cache;
            _sessionProvider = sessionProvider;
            _stopwatch = new Stopwatch();
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            // validate feature is enabled
            var config = await _cache.Configuration();
            if (!config.Session_Logging_Is_Enabled)
            {
                await next();
                return;
            }

            _stopwatch.Start();

            // do something before the action executes
            var resultContext = await next();
            // do something after the action executes; resultContext.Result will be set
            
            var session = await _sessionManager.GetSession();

            _stopwatch.Stop();

            int sessionLogId;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var methodInfo = context.HandlerMethod.MethodInfo.Name;

                var method = context.HttpContext.Request.Method;
                var isAjax = !(methodInfo == "OnGet" || methodInfo == "OnPost"
                       || methodInfo == "OnGetAsync" || methodInfo == "OnPostAsync");

                var dbRequest = new Repositories.DatabaseRepos.SessionRepo.Models.CreateSessionLogRequest()
                {
                    Session_Id = session.SessionEntity.Id,
                    Page = (string)context.RouteData.Values["Page"],
                    Handler_Name = context.HandlerMethod.Name,
                    Method = method,
                    Controller = (string)context.RouteData.Values["Controller"],
                    Action = (string)context.RouteData.Values["Action"],
                    IsAJAX = isAjax,
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    Elapsed_Milliseconds = _stopwatch.ElapsedMilliseconds,
                    Created_By = ApplicationConstants.SystemUserId
                };

                if (method == "POST" || !string.IsNullOrEmpty(context.HandlerMethod.Name))
                {
                    var postData = new Dictionary<string, object>();

                    var hasHandlerArguments = context.HandlerArguments.Any();
                    if (hasHandlerArguments)
                    {
                        postData.Add("Handler_Arguments", context.HandlerArguments);
                    }

                    var formData = context.HandlerInstance.GetType().GetProperty("FormData");
                    var hasFormData = formData != null;
                    if (hasFormData && !isAjax)
                    {
                        postData.Add("Form_Data", ((dynamic)context.HandlerInstance).FormData);
                    }

                    if (hasHandlerArguments || hasFormData)
                    {
                        var jsonString = JsonConvert.SerializeObject(postData, Formatting.Indented).Trim();
                        dbRequest.Action_Data_JSON = JsonHelper.ObfuscateFieldValues(jsonString, ApplicationConstants.ObfuscatedActionArgumentFields);
                    }
                }

                sessionLogId = await uow.SessionRepo.CreateSessionLog(dbRequest);
                uow.Commit();

                // required for session event logging
                await _sessionProvider.Set(SessionConstants.SessionLogId, sessionLogId);

                if (resultContext.Exception != null && !resultContext.ExceptionHandled)
                {
                    await _sessionManager.WriteSessionLogEvent(new Models.ManagerModels.Session.CreateSessionLogEventRequest()
                    {
                        EventKey = SessionEventKeys.Error,
                        Info = new Dictionary<string, string>()
                    {
                        { "Message", resultContext.Exception.Message }
                    }
                    });
                }
            }
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {

        }
    }
}
