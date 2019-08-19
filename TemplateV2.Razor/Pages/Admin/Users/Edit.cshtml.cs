using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin.Users;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditUserModel : BasePageModel
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly ICacheManager _cache;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdateUserRequest FormData { get; set; }

        public UserEntity UserEntity { get; set; }

        public List<RoleEntity> RolesLookup { get; set; }

        #endregion

        #region Constructors

        public EditUserModel(IUserService userService, ICacheManager cache)
        {
            _userService = userService;
            _cache = cache;
            RolesLookup = new List<RoleEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _userService.GetUser(new GetUserRequest()
            {
                Id = Id
            });
            RolesLookup = await _cache.Roles();
            UserEntity = response.User;
            FormData = new UpdateUserRequest()
            {
                Username = response.User.Username,
                EmailAddress = response.User.Email_Address,
                FirstName = response.User.First_Name,
                LastName = response.User.Last_Name,
                MobileNumber = response.User.Mobile_Number,
                RoleIds = response.Roles.Select(r => r.Id).ToList()
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                FormData.Id = Id;
                var response = await _userService.UpdateUser(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/Users/Index");
                }
                AddFormErrors(response);
            }
            var userResponse = await _userService.GetUser(new GetUserRequest()
            {
                Id = Id
            });
            RolesLookup = await _cache.Roles();
            UserEntity = userResponse.User;
            return Page();
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

        public async Task<IActionResult> OnPostConfirmRegistration(int id)
        {
            var response = await _userService.ConfirmRegistration(new Models.ServiceModels.Admin.Users.ConfirmRegistrationRequest()
            {
                Id = id
            });
            AddNotifications(response);
            return RedirectToPage("/Admin/Users/Index");
        }

        public async Task<IActionResult> OnPostGenerateResetPasswordUrl([FromBody] GenerateResetPasswordUrlRequest request)
        {
            ModelState.Clear(); // needed to prevent other forms being included in validation
            if (TryValidateModel(request))
            {
                var response = await _userService.GenerateResetPasswordUrl(request);
                if (response.IsSuccessful)
                {
                    return new JsonResult(response);
                }
                AddFormErrors(response);
                return new UnprocessableEntityObjectResult(ModelState);
            }
            return new BadRequestObjectResult(ModelState);
        }

        public async Task<IActionResult> OnPostSendResetPasswordEmail([FromBody] SendResetPasswordEmailRequest request)
        {
            ModelState.Clear(); // needed to prevent other forms being included in validation
            if (TryValidateModel(request))
            {
                var response = await _userService.SendResetPasswordEmail(request);
                if (response.IsSuccessful)
                {
                    return new JsonResult(response);
                }
                AddFormErrors(response);
                return new UnprocessableEntityObjectResult(ModelState);
            }
            return new BadRequestObjectResult(ModelState);
        }
    }
}
