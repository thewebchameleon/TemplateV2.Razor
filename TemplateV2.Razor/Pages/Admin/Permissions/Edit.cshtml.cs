using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Permissions;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditPermissionModel : BasePageModel
    {
        #region Private Fields

        private readonly IPermissionsService _permissionsService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdatePermissionRequest FormData { get; set; }

        public string Key { get; set; }

        #endregion

        #region Constructors

        public EditPermissionModel(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        #endregion

        public async Task<IActionResult> OnGet()
        {
            var response = await _permissionsService.GetPermission(new GetPermissionRequest()
            {
                Id = Id
            });

            if (!response.IsSuccessful)
            {
                return NotFound();
            }

            Key = response.Permission.Key;
            FormData = new UpdatePermissionRequest()
            {
                Id = response.Permission.Id,
                Description = response.Permission.Description,
                GroupName = response.Permission.Group_Name,
                Name = response.Permission.Name
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                FormData.Id = Id;
                var response = await _permissionsService.UpdatePermission(FormData);
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
