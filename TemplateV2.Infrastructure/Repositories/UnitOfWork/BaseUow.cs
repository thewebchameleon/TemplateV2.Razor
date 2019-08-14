using TemplateV2.Infrastructure.Configuration.Models;

namespace TemplateV2.Infrastructure.Repositories.UnitOfWork
{
    public class BaseUow
    {
        #region Instance Fields

        private readonly ConnectionStringSettings _connectionStrings;

        #endregion

        #region Constructor

        public BaseUow(ConnectionStringSettings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        #endregion

        #region Properties

        protected virtual string DefaultUowConnectionString
        {
            get
            {
                return _connectionStrings.DefaultConnection; // specifies a specific connection string
            }
        }

        #endregion
    }
}
