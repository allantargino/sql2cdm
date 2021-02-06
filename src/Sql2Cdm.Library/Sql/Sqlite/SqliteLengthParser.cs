using Sql2Cdm.Library.Models;
using System.Text.RegularExpressions;

namespace Sql2Cdm.Library.Sql.Sqlite
{
    public static class SqliteLengthParser
    {
        private static readonly Regex removeIdentityRegex = new Regex(@"IDENTITY\(.*\)", RegexOptions.Compiled);
        private static readonly Regex digitsRegex = new Regex(@"\((\d*),?(\d*)\)", RegexOptions.Compiled);

        public static ColumnLength GetSqlLengthFromString(string type)
        {
            var columnLength = new ColumnLength();

            if (string.IsNullOrWhiteSpace(type))
                return columnLength;

            string processed = PreProcessType(type);

            var matches = digitsRegex.Match(processed);

            if (matches.Length > 0 && matches.Groups.Count >= 3)
            {
                _ = int.TryParse(matches.Groups[1].Value, out int d1);
                _ = int.TryParse(matches.Groups[2].Value, out int d2);

                if (d1 > 0 && d2 > 0)
                {
                    return columnLength with { Precision = d1, Digits = d2 };
                }
                else
                {
                    return columnLength with { MaxSize = d1 };
                }
            }

            return columnLength;
        }

        private static string PreProcessType(string type)
        {
            var temp = type.Trim().ToUpper();
            return removeIdentityRegex.Replace(temp, string.Empty);
        }
    }
}
