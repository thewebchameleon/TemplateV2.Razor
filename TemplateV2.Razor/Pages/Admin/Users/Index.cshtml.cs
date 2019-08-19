using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageUsersModel : BasePageModel
    {
        #region Private Fields

        private readonly IUserService _userService;

        #endregion

        #region Properties


        #endregion

        #region Constructors

        public ManageUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task OnGet()
        {

        }

        public async Task<JsonResult> OnGetData()
        {
            var response = await _userService.GetUsers();
            return new JsonResult(response);
        }

        public async Task<IActionResult> OnPostDisableUser(int id)
        {
            var response = await _userService.DisableUser(new Models.ServiceModels.Admin.Users.DisableUserRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }

        public async Task<IActionResult> OnPostEnableUser(int id)
        {
            var response = await _userService.EnableUser(new Models.ServiceModels.Admin.Users.EnableUserRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }

        public async Task<IActionResult> OnPostUnlockUser(int id)
        {
            var response = await _userService.UnlockUser(new Models.ServiceModels.Admin.Users.UnlockUserRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }
    }
}
