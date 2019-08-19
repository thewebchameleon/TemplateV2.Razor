using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageRolesModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties


        #endregion

        #region Constructors

        public ManageRolesModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #endregion

        public async Task OnGet()
        {
        }

        public async Task<JsonResult> OnGetData()
        {
            var response = await _adminService.GetRoles();
            return new JsonResult(response);
        }

        public async Task<IActionResult> OnPostDisableRole(int id)
        {
            var response = await _adminService.DisableRole(new Models.ServiceModels.Admin.Roles.DisableRoleRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Roles/Index");
        }

        public async Task<IActionResult> OnPostEnableRole(int id)
        {
            var response = await _adminService.EnableRole(new Models.ServiceModels.Admin.Roles.EnableRoleRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Roles/Index");
        }
    }
}
