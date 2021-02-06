using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Sql2Cdm.Library.Sql.Sqlite
{
    public static class SqliteSqlDbTypeParser
    {
        private static readonly Regex regex = new Regex(@"^(\w*?)\s*(IDENTITY)?(\(.*\))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static SqlDbType? GetSqlTypeFromString(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;

            var regexMatch = regex.Match(type);
            var sqlType = regexMatch.Groups[1].Value.Trim();

            return (SqlDbType)Enum.Parse(typeof(SqlDbType), sqlType, ignoreCase: true);
        }
    }
}
