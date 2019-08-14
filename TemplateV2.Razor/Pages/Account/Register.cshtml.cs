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
    public class RegisterModel : BasePageModel
    {
        #region Private Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Properties

        [BindProperty]
        public RegisterRequest FormData { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        #endregion

        #region Constructors

        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion


        public void OnGet()
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            FormData = new RegisterRequest();
        }

        public async Task<IActionResult> OnPost()
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            if (ModelState.IsValid)
            {
                var response = await _accountService.Register(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToHome(ReturnUrl);
                }
                AddFormErrors(response);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
