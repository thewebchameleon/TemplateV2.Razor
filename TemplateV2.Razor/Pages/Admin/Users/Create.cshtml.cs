using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin.Users;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class CreateUserModel : BasePageModel
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly ICacheManager _cache;

        #endregion

        #region Properties

        [BindProperty]
        public CreateUserRequest FormData { get; set; }

        public List<RoleEntity> RolesLookup { get; set; }

        #endregion

        #region Constructors

        public CreateUserModel(IUserService userService, ICacheManager cache)
        {
            _userService = userService;
            _cache = cache;
            RolesLookup = new List<RoleEntity>();
            FormData = new CreateUserRequest();
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
                var response = await _userService.CreateUser(FormData);
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
