using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Models.DomainModels;
using TemplateV2.Models.ServiceModels.Admin.Sessions;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class ViewSessionModel : BasePageModel
    {
        #region Private Fields

        private readonly ISessionService _sessionService;

        #endregion

        #region Properties

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }


        public UserEntity? UserEntity { get; set; }

        public SessionEntity? SessionEntity { get; set; }

        #endregion

        #region Constructors

        public ViewSessionModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #endregion

        public async Task<IActionResult> OnGet()
        {
            var response = await _sessionService.GetSession(new GetSessionRequest()
            {
                Id = Id
            });

            if (!response.IsSuccessful)
            {
                return NotFound();
            }

            UserEntity = response.User;
            SessionEntity = response.Session;

            return Page();
        }

        public async Task<JsonResult> OnGetData(int id)
        {
            var response = await _sessionService.GetSessionLogs(new GetSessionLogsRequest()
            {
                Session_Id = id
            });

            return new JsonResult(response);
        }
    }
}
