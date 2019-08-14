using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Sessions;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageSessionsModel : PageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        public int SessionExpirationMinutes { get; set; }

        #endregion

        #region Constructors

        public ManageSessionsModel(IAdminService adminService)
        {
            _adminService = adminService;
            SessionExpirationMinutes = 1;
        }

        #endregion

        public async Task OnGet()
        {
       
        }

        public async Task<JsonResult> OnGetLastXDays(int days)
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                LastXDays = days
            });

            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetFilterByDate(DateTime date)
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                Day = date
            });

            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetFilterByUserId(int userId)
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                UserId = userId
            });

            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetFilterByMobileNumber(string mobileNumber)
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                MobileNumber = mobileNumber
            });

            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetFilterByUsername(string username)
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                Username = username
            });

            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetRecent()
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                Last24Hours = true
            });

            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetFilterByEmailAddress(string emailAddress)
        {
            var response = await _adminService.GetSessions(new GetSessionsRequest()
            {
                EmailAddress = emailAddress
            });

            return new JsonResult(response);
        }
    }
}
