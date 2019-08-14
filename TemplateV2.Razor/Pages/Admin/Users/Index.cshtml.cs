using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageUsersModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties


        #endregion

        #region Constructors

        public ManageUsersModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #endregion

        public async Task OnGet()
        {

        }

        public async Task<JsonResult> OnGetData()
        {
            var response = await _adminService.GetUserManagement();
            return new JsonResult(response);
        }

        public async Task<IActionResult> OnPostDisableUser(int id)
        {
            var response = await _adminService.DisableUser(new Models.ServiceModels.Admin.Users.DisableUserRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }

        public async Task<IActionResult> OnPostEnableUser(int id)
        {
            var response = await _adminService.EnableUser(new Models.ServiceModels.Admin.Users.EnableUserRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }

        public async Task<IActionResult> OnPostUnlockUser(int id)
        {
            var response = await _adminService.UnlockUser(new Models.ServiceModels.Admin.Users.UnlockUserRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }
    }
}
