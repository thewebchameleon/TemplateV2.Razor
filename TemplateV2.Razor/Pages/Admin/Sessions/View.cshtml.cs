using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin.Sessions;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ViewSessionModel : BasePageModel
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }


        public UserEntity UserEntity { get; set; }

        public SessionEntity SessionEntity { get; set; }

        #endregion

        #region Constructors

        public ViewSessionModel(IAdminService adminService, IApplicationCache cache)
        {
            _adminService = adminService;
        }

        #endregion

        public async Task OnGet()
        {
            var response = await _adminService.GetSession(new GetSessionRequest()
            {
                Id = Id
            });
            UserEntity = response.User;
            SessionEntity = response.Session;
        }

        public async Task<JsonResult> OnGetData(int id)
        {
            var response = await _adminService.GetSessionLogs(new GetSessionLogsRequest()
            {
                Session_Id = id
            });

            return new JsonResult(response);
        }
    }
}
