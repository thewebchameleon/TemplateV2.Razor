using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageRolesModel : BasePageModel
    {
        #region Private Fields

        private readonly IRoleService _roleService;

        #endregion

        #region Properties


        #endregion

        #region Constructors

        public ManageRolesModel(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        public async Task OnGet()
        {
        }

        public async Task<JsonResult> OnGetData()
        {
            var response = await _roleService.GetRoles();
            return new JsonResult(response);
        }

        public async Task<IActionResult> OnPostDisableRole(int id)
        {
            var response = await _roleService.DisableRole(new Models.ServiceModels.Admin.Roles.DisableRoleRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Roles/Index");
        }

        public async Task<IActionResult> OnPostEnableRole(int id)
        {
            var response = await _roleService.EnableRole(new Models.ServiceModels.Admin.Roles.EnableRoleRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Roles/Index");
        }
    }
}
