using Microsoft.Extensions.Options;
using TemplateV2.Infrastructure.Configuration.Models;

namespace TemplateV2.Repositories.DatabaseRepos
{
    public class BaseSQLRepo
    {
        private readonly ConnectionStringSettings _connectionStrings;

        public BaseSQLRepo(ConnectionStringSettings connectionStringsSettings)
        {
            _connectionStrings = connectionStringsSettings;
        }

        protected virtual string DefaultConnectionString
        {
            get
            {
                return _connectionStrings.DefaultConnection; // specifies a specific connection string
            }
        }

        protected virtual int DefaultTimeOut
        {
            get
            {
                return _connectionStrings.Timeout;
            }
        }
    }
}
