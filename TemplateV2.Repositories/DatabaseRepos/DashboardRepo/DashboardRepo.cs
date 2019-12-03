using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Adapters;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Repositories.DatabaseRepos.DashboardRepo.Contracts;
using TemplateV2.Repositories.DatabaseRepos.DashboardRepo.Models;

namespace TemplateV2.Repositories.DatabaseRepos.DashboardRepo
{
    public class DashboardRepo : BaseSQLRepo, IDashboardRepo
    {
        #region Instance Fields

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        #endregion

        #region Constructor

        public DashboardRepo(IDbConnection connection, IDbTransaction transaction, ConnectionStringSettings connectionStringsSettings)
            : base(connectionStringsSettings)
        {
            _connection = connection;
            _transaction = transaction;
        }

        #endregion

        #region Public Methods

        public async Task<GetDashboardResponse> GetDashboard()
        {
            var sqlStoredProc = "sp_dashboard_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<GetDashboardResponse>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        #endregion
    }
}
