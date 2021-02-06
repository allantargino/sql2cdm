using Sql2Cdm.Library.Models;
using System.Linq;

namespace Sql2Cdm.Library.Tests.Sql.Sqlite.Extensions
{
    public static class SqliteTestExtensions
    {
        public static Table GetTable(this RelationalModel model)
        {
            return model.Tables.First();
        }

        public static Table GetTable(this RelationalModel model, string tableName)
        {
            return model.Tables.First(t => t.Name == tableName);
        }

        public static Column GetColumn(this RelationalModel model)
        {
            return model.GetTable().Columns.First();
        }

        public static Column GetColumn(this RelationalModel model, string tableName, string columnName)
        {
            return model.GetTable(tableName).Columns.First(c => c.Name == columnName);
        }
    }
}
