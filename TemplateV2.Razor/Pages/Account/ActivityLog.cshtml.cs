using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Account;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ActivityLogModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Properties

        public UserEntity UserEntity { get; set; }

        public List<ActivityLog> ActivityLogs { get; set; }

        #endregion

        #region Constructors

        public ActivityLogModel(IAccountService accountService)
        {
            _accountService = accountService;
            ActivityLogs = new List<ActivityLog>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _accountService.GetProfile();
            UserEntity = response.User;
        }

        public async Task<JsonResult> OnGetData(int id)
        {
            var response = await _accountService.GetActivityLogs(new GetActivityLogsRequest());
            return new JsonResult(response);
        }
    }
}
