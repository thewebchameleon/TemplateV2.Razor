using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Authentication;
using TemplateV2.Models.ServiceModels.Admin.Users;
using TemplateV2.Razor.Attributes;
using TemplateV2.Services.Contracts;

namespace TemplateV2.Razor.Controllers
{
    [AuthorizePermission(PermissionKeys.ManageUsers)]
    [Route("[controller]/[action]")]
    public class AdminController : BaseController
    {
        #region Instance Fields

        private readonly IAdminService _service;

        #endregion

        #region Constructors

        public AdminController(
            IAdminService service)
        {
            _service = service;
        }

        #endregion

        #region Public Methods

       

        #endregion
    }
}
