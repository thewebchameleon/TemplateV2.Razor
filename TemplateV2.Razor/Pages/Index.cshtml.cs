using System.Threading.Tasks;
using TemplateV2.Services.Admin.Contracts;

namespace TemplateV2.Razor.Pages
{
    public class IndexModel : BasePageModel
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IRoleService _roleService;
        private readonly IConfigurationService _configurationService;

        #endregion

        #region Properties

        public int TotalSessions { get; set; }

        public int TotalUsers { get; set; }

        public int TotalRoles { get; set; }

        public int TotalConfigItems { get; set; }

        #endregion

        #region Constructors

        public IndexModel(
            IUserService userService,
            ISessionService sessionService,
            IRoleService roleService,
            IConfigurationService configurationService
        )
        {
            _userService = userService;
            _sessionService = sessionService;
            _roleService = roleService;
            _configurationService = configurationService;
        }

        #endregion

        public async Task OnGet()
        {
            // note: this is inefficient and is merely for demonstrating the UI
            // we should rather build a custom stored proc(s) to get the data we need
            // and even cache it if necessary
            var usersResponse = await _userService.GetUsers();
            TotalUsers = usersResponse.Users.Count;

            var sessionsResponse = await _sessionService.GetSessions(new Models.ServiceModels.Admin.Sessions.GetSessionsRequest()
            {
                LastXDays = 9999
            }) ;
            TotalSessions = sessionsResponse.Sessions.Count;

            var rolesResponse = await _roleService.GetRoles();
            TotalRoles = rolesResponse.Roles.Count;

            var configItems = await _configurationService.GetConfigurationItems();
            TotalConfigItems = configItems.ConfigurationItems.Count;
        }
    }
}
