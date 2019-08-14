using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin;
using TemplateV2.Models.ServiceModels.Admin.Configuration;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditConfigurationModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdateConfigurationItemRequest FormData { get; set; }

        public string Key { get; set; }

        #endregion

        #region Constructors

        public EditConfigurationModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _adminService.GetConfigurationItem(new GetConfigurationItemRequest()
            {
                Id = Id
            });
            Key = response.ConfigurationItem.Key;
            FormData = new UpdateConfigurationItemRequest()
            {
                Id = response.ConfigurationItem.Id,
                Description = response.ConfigurationItem.Description,
                BooleanValue = response.ConfigurationItem.Boolean_Value,
                DateTimeValue = response.ConfigurationItem.DateTime_Value,
                DecimalValue = response.ConfigurationItem.Decimal_Value,
                IntValue = response.ConfigurationItem.Int_Value,
                MoneyValue = response.ConfigurationItem.Money_Value,
                StringValue = response.ConfigurationItem.String_Value,
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                FormData.Id = Id;
                var response = await _adminService.UpdateConfigurationItem(FormData);
                if (response.IsSuccessful)
                {
                    AddNotifications(response);
                    return RedirectToPage("/Admin/Configuration/Index");
                }
                AddFormErrors(response);
            }
            return Page();
        }
    }
}
