using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Authentication;
using TemplateV2.Infrastructure.Cache;
using TemplateV2.Infrastructure.Cache.Contracts;
using TemplateV2.Infrastructure.Configuration;
using TemplateV2.Infrastructure.Session;
using TemplateV2.Infrastructure.Session.Contracts;
using TemplateV2.Models.ServiceModels;
using TemplateV2.Models.ServiceModels.Admin;
using TemplateV2.Repositories.UnitOfWork.Contracts;
using TemplateV2.Services.Admin.Contracts;
using TemplateV2.Services.Contracts;
using TemplateV2.Services.Managers.Contracts;

namespace TemplateV2.Services.Admin
{
    public class AdminService : IAdminService
    {
        #region Instance Fields

        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;
        private readonly ISessionProvider _sessionProvider;

        private readonly IAuthenticationManager _authenticationManager;

        private readonly IUnitOfWorkFactory _uowFactory;

        private readonly ICacheProvider _cacheProvider;

        #endregion

        #region Constructor

        public AdminService(
            IAccountService accountService,
            ISessionManager sessionManager,
            ISessionProvider sessionProvider,
            IUnitOfWorkFactory uowFactory,
            ICacheProvider cacheProvider,
             IAuthenticationManager authenticationManager)
        {
            _uowFactory = uowFactory;
            _cacheProvider = cacheProvider;
            _accountService = accountService;
            _sessionManager = sessionManager;
            _sessionProvider = sessionProvider;
            _authenticationManager = authenticationManager;
        }

        #endregion

        #region Public Methods

        public async Task<CreateAdminUserResponse> CreateAdminUser(CreateAdminUserRequest request)
        {
            var response = new CreateAdminUserResponse();
            var username = request.Username;
            var session = await _sessionManager.GetSession();

            var duplicateResponse = await _accountService.DuplicateUserCheck(new DuplicateUserCheckRequest()
            {
                Username = username
            });

            if (duplicateResponse.Notifications.HasErrors)
            {
                response.Notifications.Add(duplicateResponse.Notifications);
                return response;
            }

            int userId;
            using (var uow = _uowFactory.GetUnitOfWork())
            {
                userId = await uow.UserRepo.CreateUser(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserRequest()
                {
                    Username = username,
                    First_Name = username,
                    Password_Hash = PasswordHelper.HashPassword(request.Password),
                    Created_By = ApplicationConstants.SystemUserId,
                    Registration_Confirmed = true,
                    Is_Enabled = true
                });

                await uow.UserRepo.CreateUserRole(new Repositories.DatabaseRepos.UserRepo.Models.CreateUserRoleRequest()
                {
                    User_Id = userId,
                    Role_Id = 1, // the first role should always be admin
                    Created_By = ApplicationConstants.SystemUserId
                });

                var sessionEntity = await uow.SessionRepo.AddUserToSession(new Repositories.DatabaseRepos.SessionRepo.Models.AddUserToSessionRequest()
                {
                    Id = session.SessionEntity.Id,
                    User_Id = userId,
                    Updated_By = ApplicationConstants.SystemUserId
                });
                await _sessionProvider.Set(SessionConstants.SessionEntity, sessionEntity);
                uow.Commit();
            }

            _cacheProvider.Set(CacheConstants.RequiresAdminUser, false);
            await _authenticationManager.SignIn(session.SessionEntity.Id);

            return response;
        }

        #endregion
    }
}
