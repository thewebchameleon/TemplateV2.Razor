using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class LoginModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Properties

        [BindProperty]
        public LoginRequest FormData { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public bool ShowPassword { get; set; }

        #endregion

        #region Constructors

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
            FormData = new LoginRequest();
        }

        #endregion

        public void OnGet()
        {
            FormData = new LoginRequest();
            ViewData["ReturnUrl"] = ReturnUrl;
        }

        public async Task<IActionResult> OnPost(string returnUrl)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(FormData);
                if (response.IsSuccessful)
                {
                    return RedirectToHome(ReturnUrl);
                }
                AddFormErrors(response);
            }

            // If we got this far, something failed, redisplay form
            ViewData["ReturnUrl"] = ReturnUrl;
            return Page();
        }
    }
}
