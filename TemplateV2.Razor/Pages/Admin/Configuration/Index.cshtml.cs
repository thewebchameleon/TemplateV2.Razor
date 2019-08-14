using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ManageConfigurationModel : PageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        public List<ConfigurationEntity> ConfigurationItems { get; set; }

        #endregion

        #region Constructors

        public ManageConfigurationModel(IAdminService adminService)
        {
            _adminService = adminService;
            ConfigurationItems = new List<ConfigurationEntity>();
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _adminService.GetConfigurationManagement();
            ConfigurationItems = response.ConfigurationItems;
        }
    }
}
