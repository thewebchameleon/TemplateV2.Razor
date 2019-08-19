using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin.Users;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class CreateUserModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;
        private readonly ICacheManager _cache;

        #endregion

        #region Properties

        [BindProperty]
        public CreateUserRequest FormData { get; set; }

        public List<RoleEntity> RolesLookup { get; set; }

        #endregion

        #region Constructors

        public CreateUserModel(IAdminService adminService, ICacheManager cache)
        {
            _adminService = adminService;
            _cache = cache;
            RolesLookup = new List<RoleEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            RolesLookup = await _cache.Roles();
            FormData = new CreateUserRequest();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _adminService.CreateUser(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/Users/Index");
                }
                AddFormErrors(response);
            }
            RolesLookup = await _cache.Roles();
            return Page();
        }
    }
}
