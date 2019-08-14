using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Services.Contracts;
using TemplateV2.Models.ServiceModels.Admin.Roles;
using TemplateV2.Models.DomainModels;
using System.Collections.Generic;
using System.Linq;
using TemplateV2.Infrastructure.Cache.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditRoleModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;
        private readonly IApplicationCache _cache;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdateRoleRequest FormData { get; set; }

        public RoleEntity RoleEntity { get; set; }

        public List<PermissionEntity> PermissionsLookup { get; set; }

        #endregion

        #region Constructors

        public EditRoleModel(IAdminService adminService, IApplicationCache cache)
        {
            _adminService = adminService;
            _cache = cache;
            PermissionsLookup = new List<PermissionEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _adminService.GetRole(new GetRoleRequest()
            {
                Id = Id
            });
            PermissionsLookup = await _cache.Permissions();
            RoleEntity = response.Role;
            FormData = new UpdateRoleRequest()
            {
                Id = response.Role.Id,
                Description = response.Role.Description,
                Name = response.Role.Name,
                PermissionIds = response.Permissions.Select(c => c.Id).ToList()
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                FormData.Id = Id;
                var response = await _adminService.UpdateRole(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/Roles/Index");
                }
                AddFormErrors(response);
            }
            var roleResponse = await _adminService.GetRole(new GetRoleRequest()
            {
                Id = Id
            });
            PermissionsLookup = await _cache.Permissions();
            RoleEntity = roleResponse.Role;
            return Page();
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
