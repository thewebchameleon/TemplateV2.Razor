using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Account;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ProfileModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _service;

        #endregion

        #region Properties

        [BindProperty]
        public UpdateProfileRequest FormData { get; set; }

        public UserEntity? UserEntity { get; set; }

        public List<RoleEntity> Roles { get; set; }

        #endregion

        #region Constructors

        public ProfileModel(IAccountService service)
        {
            _service = service;
            Roles = new List<RoleEntity>();
            FormData = new UpdateProfileRequest();
        }

        #endregion


        public async Task OnGet()
        {
            var response = await _service.GetProfile();
            FormData = new UpdateProfileRequest()
            {
                EmailAddress = response.User.Email_Address,
                FirstName = response.User.First_Name,
                LastName = response.User.Last_Name,
                MobileNumber = response.User.Mobile_Number,
                Username = response.User.Username
            };
            Roles = response.Roles;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _service.UpdateProfile(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToHome();
                }
                AddFormErrors(response);
            }

            var profileResponse = await _service.GetProfile();
            Roles = profileResponse.Roles;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            ModelState.Clear(); // needed to prevent other forms being included in validation
            if (TryValidateModel(request))
            {
                var response = await _service.UpdatePassword(request);
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
