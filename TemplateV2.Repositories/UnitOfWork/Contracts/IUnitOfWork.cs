using System;
using TemplateV2.Repositories.DatabaseRepos.ConfigurationRepo.Contracts;
using TemplateV2.Repositories.DatabaseRepos.DashboardRepo.Contracts;
using TemplateV2.Repositories.DatabaseRepos.SessionRepo.Contracts;
using TemplateV2.Repositories.DatabaseRepos.UserRepo.Contracts;

namespace TemplateV2.Repositories.UnitOfWork.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IConfigurationRepo ConfigurationRepo { get; }

        ISessionRepo SessionRepo { get; }

        IUserRepo UserRepo { get; }

        IDashboardRepo DashboardRepo { get; }

        bool Commit();
    }
}
