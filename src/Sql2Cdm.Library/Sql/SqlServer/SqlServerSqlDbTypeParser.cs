using System;
using System.Data;

namespace Sql2Cdm.Library.Sql.SqlServer
{
    public static class SqlServerSqlDbTypeParser
    {
        public static SqlDbType? GetSqlTypeFromString(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;

            return type switch
            {
                "numeric" => SqlDbType.Int,
                _ => (SqlDbType)Enum.Parse(typeof(SqlDbType), type, ignoreCase: true),
            };
        }
    }
}
