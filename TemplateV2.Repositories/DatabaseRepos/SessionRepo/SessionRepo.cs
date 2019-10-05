using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.Adapters;
using TemplateV2.Infrastructure.Configuration.Models;
using TemplateV2.Repositories.DatabaseRepos.SessionRepo.Models;
using TemplateV2.Models.DomainModels;
using TemplateV2.Repositories.DatabaseRepos.SessionRepo.Contracts;
using Microsoft.Extensions.Options;

namespace TemplateV2.Repositories.DatabaseRepos.SessionRepo
{
    public class SessionRepo : BaseSQLRepo, ISessionRepo
    {
        #region Instance Fields

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        #endregion

        #region Constructor

        public SessionRepo(IDbConnection connection, IDbTransaction transaction, ConnectionStringSettings connectionStringsSettings)
            : base(connectionStringsSettings)
        {
            _connection = connection;
            _transaction = transaction;
        }

        #endregion

        #region Public Methods

        public async Task<SessionEntity> AddUserToSession(AddUserToSessionRequest request)
        {
            var sqlStoredProc = "sp_session_add_user_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<SessionEntity> CreateSession(CreateSessionRequest request)
        {
            var sqlStoredProc = "sp_session_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<List<GetSessionsResponse>> GetAdminSessionsByDate(GetSessionsByDateRequest request)
        {
            var sqlStoredProc = "sp_sessions_get_by_date";

            var response = await DapperAdapter.GetFromStoredProcAsync<GetSessionsResponse>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<GetSessionsResponse>> GetAdminSessionsByStartDate(GetSessionsByStartDateRequest request)
        {
            var sqlStoredProc = "sp_sessions_get_by_start_date";

            var response = await DapperAdapter.GetFromStoredProcAsync<GetSessionsResponse>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<GetSessionsResponse>> GetAdminSessionsByUserId(GetSessionsByUserIdRequest request)
        {
            var sqlStoredProc = "sp_sessions_get_by_user_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<GetSessionsResponse>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }


        public async Task<List<SessionLogEntity>> GetSessionLogsBySessionId(GetSessionLogsBySessionIdRequest request)
        {
            var sqlStoredProc = "sp_session_logs_get_by_session_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionLogEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<SessionLogEventEntity>> GetSessionLogEventsBySessionId(GetSessionLogEventsBySessionIdRequest request)
        {
            var sqlStoredProc = "sp_session_log_events_get_by_session_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionLogEventEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }


        public async Task<List<SessionLogEventEntity>> GetSessionLogEventsByUserId(GetSessionLogEventsByUserIdRequest request)
        {
            var sqlStoredProc = "sp_session_log_events_get_by_user_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionLogEventEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task<List<SessionEventEntity>> GetSessionEvents()
        {
            var sqlStoredProc = "sp_session_events_get";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionEventEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: new { },
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.ToList();
        }

        public async Task UpdateSessionEvent(UpdateSessionEventRequest request)
        {
            var sqlStoredProc = "sp_session_event_update";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items have been updated");
            }
        }

        public async Task<int> CreateSessionEvent(CreateSessionEventRequest request)
        {
            var sqlStoredProc = "sp_session_event_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items have been created");
            }
            return response.FirstOrDefault();
        }

        public async Task<SessionEntity> GetSessionById(GetSessionByIdRequest request)
        {
            var sqlStoredProc = "sp_session_get_by_id";

            var response = await DapperAdapter.GetFromStoredProcAsync<SessionEntity>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            return response.FirstOrDefault();
        }

        public async Task<int> CreateSessionLog(CreateSessionLogRequest request)
        {
            var sqlStoredProc = "sp_session_log_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items have been created");
            }
            return response.FirstOrDefault();
        }

        public async Task<int> CreateSessionLogEvent(CreateSessionLogEventRequest request)
        {
            var sqlStoredProc = "sp_session_log_event_create";

            var response = await DapperAdapter.GetFromStoredProcAsync<int>
                (
                    storedProcedureName: sqlStoredProc,
                    parameters: request,
                    dbconnectionString: DefaultConnectionString,
                    sqltimeout: DefaultTimeOut,
                    dbconnection: _connection,
                    dbtransaction: _transaction);

            if (response == null || response.FirstOrDefault() == 0)
            {
                throw new Exception("No items have been created");
            }
            return response.FirstOrDefault();
        }




        #endregion
    }
}
