using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Permissions;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class CreatePermissionModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        [BindProperty]
        public CreatePermissionRequest FormData { get; set; }

        #endregion

        #region Constructors

        public CreatePermissionModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #endregion

        public async Task OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _adminService.CreatePermission(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/Permissions/Index");
                }
                AddFormErrors(response);
            }
            return Page();
        }
    }
}
