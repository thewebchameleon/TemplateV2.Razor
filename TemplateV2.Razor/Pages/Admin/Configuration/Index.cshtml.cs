using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageConfigurationModel : PageModel
    {
        #region Private Fields

        private readonly IConfigurationService _configService;

        #endregion

        #region Properties

        public List<ConfigurationEntity> ConfigurationItems { get; set; }

        #endregion

        #region Constructors

        public ManageConfigurationModel(IConfigurationService adminService)
        {
            _configService = adminService;
            ConfigurationItems = new List<ConfigurationEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _configService.GetConfigurationItems();
            ConfigurationItems = response.ConfigurationItems;
        }
    }
}
