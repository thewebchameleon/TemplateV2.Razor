using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin.Roles;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class CreateRoleModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;
        private readonly IApplicationCache _cache;

        #endregion

        #region Properties

        [BindProperty]
        public CreateRoleRequest FormData { get; set; }

        public List<PermissionEntity> PermissionsLookup { get; set; }

        #endregion

        #region Constructors

        public CreateRoleModel(IAdminService adminService, IApplicationCache cache)
        {
            _adminService = adminService;
            _cache = cache;
            PermissionsLookup = new List<PermissionEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            PermissionsLookup = await _cache.Permissions();
            FormData = new CreateRoleRequest();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _adminService.CreateRole(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/Roles/Index");
                }
                AddFormErrors(response);
            }
            return Page();
        }
    }
}
