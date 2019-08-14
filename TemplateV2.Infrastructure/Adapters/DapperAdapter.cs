using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TemplateV2.Infrastructure.Adapters
{
    public class DapperAdapter
    {
        private static IDbConnection OpenDBConnection(string connectionString = null, IDbConnection connection = null)
        {
            if ((string.IsNullOrWhiteSpace(connectionString)) && (connection == null))
            {
                throw new ArgumentException("ConnectionString and Connection cannot both be null!");
            }

            if (connection != null)
            {
                return connection;
            }

            // Connection Pooling enabled by default, MARS (Multiple active record sets) not enabled           
            return new SqlConnection(connectionString);
        }

        private static int GetSqlTimeOut(int? timeout = null)
        {
            return timeout.Value;
        }

        public static Task<IEnumerable<T>> GetFromStoredProcAsync<T>(
            string storedProcedureName,
            object parameters = null,
            string dbconnectionString = null,
            IDbConnection dbconnection = null,
            int? sqltimeout = null,
            IDbTransaction dbtransaction = null)
        {
            var connection = OpenDBConnection(dbconnectionString, dbconnection);
            return connection.QueryAsync<T>
            (
                sql: storedProcedureName,
                param: parameters,
                commandType: CommandType.StoredProcedure,
                transaction: dbtransaction,
                commandTimeout: GetSqlTimeOut(sqltimeout)
            );
        }

        public static async Task<int> ExecuteAsStoredProc(
            string storedProcedureName,
            object parameters = null,
            string dbconnectionString = null,
            IDbConnection dbconnection = null,
            int? sqltimeout = null,
            IDbTransaction dbtransaction = null)
        {
            DynamicParameters p = new DynamicParameters(parameters);
            p.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var connection = OpenDBConnection(dbconnectionString, dbconnection);
            await connection.ExecuteAsync
            (
                sql: storedProcedureName,
                param: p,
                commandType: CommandType.StoredProcedure,
                transaction: dbtransaction,
                commandTimeout: GetSqlTimeOut(sqltimeout)
            ).ConfigureAwait(false);

            return p.Get<int>("@ReturnValue");
        }

        public async static void ExecuteAsStoredProcWithOutput(
                string storedProcedureName,
                object inputParameters = null,
                IEnumerable<string> outputParameters = null,
                string dbconnectionString = null,
                IDbConnection dbconnection = null,
                int? sqltimeout = null,
                IDbTransaction dbtransaction = null)
        {
            // Parameters
            DynamicParameters p = new DynamicParameters();
            p.AddDynamicParams(inputParameters);
            // setup output parameters

            var connection = OpenDBConnection(dbconnectionString, dbconnection);
            await connection.ExecuteAsync
            (
                sql: storedProcedureName,
                param: p,
                commandType: CommandType.StoredProcedure,
                transaction: dbtransaction,
                commandTimeout: GetSqlTimeOut(sqltimeout)
            );
        }

        public async static Task<IEnumerable<T>> ExecuteDynamicSql<T>(
                string sql,
                string dbconnectionString = null,
                IDbConnection dbconnection = null,
                int? sqltimeout = null,
                IDbTransaction dbtransaction = null)
        {
            var connection = OpenDBConnection(dbconnectionString, dbconnection);
            return await connection.QueryAsync<T>
            (
                sql: sql,
                commandType: CommandType.Text,
                transaction: dbtransaction,
                commandTimeout: GetSqlTimeOut(sqltimeout)
            );
        }
    }
}
