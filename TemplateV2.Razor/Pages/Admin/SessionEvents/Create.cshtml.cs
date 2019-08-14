using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Permissions;
using TemplateV2.Models.ServiceModels.Admin.SessionEvents;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class CreateSessionEventModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        [BindProperty]
        public CreateSessionEventRequest FormData { get; set; }

        #endregion

        #region Constructors

        public CreateSessionEventModel(IAdminService adminService)
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
                var response = await _adminService.CreateSessionEvent(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/SessionEvents/Index");
                }
                AddFormErrors(response);
            }
            return Page();
        }
    }
}
