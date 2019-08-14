using System;
using TemplateV2.Infrastructure.Repositories.DatabaseRepos.ConfigurationRepo.Contracts;
using TemplateV2.Infrastructure.Repositories.DatabaseRepos.SessionRepo.Contracts;
using TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Contracts;

namespace TemplateV2.Infrastructure.Repositories.UnitOfWork.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IConfigurationRepo ConfigurationRepo { get; }

        ISessionRepo SessionRepo { get; }

        IUserRepo UserRepo { get; }

        bool Commit();
    }
}
