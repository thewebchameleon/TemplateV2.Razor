using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.ServiceModels.Admin.Configuration;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class EditConfigurationModel : BasePageModel
    {
        #region Private Fields

        private readonly IConfigurationService _configService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdateConfigurationItemRequest FormData { get; set; }

        public string Key { get; set; }

        #endregion

        #region Constructors

        public EditConfigurationModel(IConfigurationService configService)
        {
            _configService = configService;
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _configService.GetConfigurationItem(new GetConfigurationItemRequest()
            {
                Id = Id
            });
            Key = response.ConfigurationItem.Key;
            FormData = new UpdateConfigurationItemRequest()
            {
                Id = response.ConfigurationItem.Id,
                Description = response.ConfigurationItem.Description,
                IsClientSide = response.ConfigurationItem.Is_Client_Side,
                BooleanValue = response.ConfigurationItem.Boolean_Value,
                DateTimeValue = response.ConfigurationItem.DateTime_Value,
                DateValue = response.ConfigurationItem.Date_Value,
                TimeValue = response.ConfigurationItem.Time_Value,
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
                var response = await _configService.UpdateConfigurationItem(FormData);
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
