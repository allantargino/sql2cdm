using Microsoft.Data.SqlClient;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Sql;
using Sql2Cdm.Library.Sql.SqlServer;
using System;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.SqlServer
{
    [Trait("TestCategory", "Integration")]
    public class SqlServerRelationalModelReaderTests : BaseSqlRelationalModelReaderTests
    {
        public SqlServerRelationalModelReaderTests() : base(CreateSqlConnection()) { }

        private static SqlConnection CreateSqlConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("SQL2CDM_CONNECTION_STRING");
            return new SqlConnection(connectionString);
        }

        protected override IRelationalModelReader CreateRelationalModelReader()
        {
            return new SqlServerRelationalModelReader((SqlConnection)Connection, (SqlTransaction)Transaction, new SqlRelationalModelReaderOptions());
        }
    }
}
