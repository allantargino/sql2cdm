using Microsoft.Data.Sqlite;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Sql.Sqlite;

namespace Sql2Cdm.Library.Tests.Sql.Sqlite
{
    public class SqliteRelationalModelReaderTests : BaseSqlRelationalModelReaderTests
    {
        public SqliteRelationalModelReaderTests() : base(new SqliteConnection("Filename=:memory:"))
        {
        }

        protected override IRelationalModelReader CreateRelationalModelReader()
        {
            return new SqliteRelationalModelReader((SqliteConnection)Connection);
        }
    }
}
