using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Common.Helpers;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Infrastructure.Session.Contracts;
using TemplateV2.Infrastructure.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Contracts;
using System.Collections.Generic;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Filters
{
    public class SessionLoggingFilter : IAsyncPageFilter
    {
        private readonly ISessionManager _sessionManager;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IApplicationCache _cache;
        private readonly ISessionProvider _sessionProvider;

        public SessionLoggingFilter(
            ISessionManager sessionManager,
            IApplicationCache cache,
            IUnitOfWorkFactory uowFactory,
            ISessionProvider sessionProvider)
        {
            _sessionManager = sessionManager;
            _uowFactory = uowFactory;
            _cache = cache;
            _sessionProvider = sessionProvider;
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

            var session = await _sessionManager.GetSession();

            int sessionLogId;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                var methoInfo = context.HandlerMethod.MethodInfo.Name;
                var method = context.HttpContext.Request.Method;
                var dbRequest = new Infrastructure.Repositories.DatabaseRepos.SessionRepo.Models.CreateSessionLogRequest()
                {
                    Session_Id = session.SessionEntity.Id,
                    Page = (string)context.RouteData.Values["Page"],
                    Handler_Name = context.HandlerMethod.Name,
                    Method = method,
                    Controller = (string)context.RouteData.Values["Controller"],
                    Action = (string)context.RouteData.Values["Action"],
                    IsAJAX = !(methoInfo == "OnGet" || methoInfo == "OnPost"),
                    Url = context.HttpContext.Request.GetDisplayUrl(),
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
                    if (hasFormData)
                    {
                        // todo: FormData always gets added on ajax posts :/ figure out how to filter it out
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
            }

            // do something before the action executes
            var resultContext = await next();
            // do something after the action executes; resultContext.Result will be set

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

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {

        }
    }
}
