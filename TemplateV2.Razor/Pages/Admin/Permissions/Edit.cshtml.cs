using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Permissions;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditPermissionModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdatePermissionRequest FormData { get; set; }

        public string Key { get; set; }

        #endregion

        #region Constructors

        public EditPermissionModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _adminService.GetPermission(new GetPermissionRequest()
            {
                Id = Id
            });
            Key = response.Permission.Key;
            FormData = new UpdatePermissionRequest()
            {
                Id = response.Permission.Id,
                Description = response.Permission.Description,
                GroupName = response.Permission.Group_Name,
                Name = response.Permission.Name
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                FormData.Id = Id;
                var response = await _adminService.UpdatePermission(FormData);
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
