using Sql2Cdm.Library.Sql.Annotations.DataStructures;
using System.Text.RegularExpressions;

namespace Sql2Cdm.Library.Sql.Annotations.Parser
{
    public class SqlTextAnnotationsParser
    {
        private const string CommaReplacement = "#";

        public SqlAnnotationsCollection<string> ParseTextAnnotations(string sqlText)
        {
            var results = new SqlAnnotationsCollection<string>();

            var processedSqlText = PreProcessSqlText(sqlText);

            ParseTableAnnotations(processedSqlText, results);

            return results;
        }

        private string PreProcessSqlText(string sqlText)
        {
            // Replaces ',' for '#' in cases such as IDENTITY(1,1) or DECIMAL(10,2)
            var processed = Regex.Replace(sqlText, @"\(\s*\d*\s*,\s*\d*\s*\)", m => m.Value.Replace(",", CommaReplacement));

            // Replaces ',' for '#' in cases such as /* {something,abc} */
            processed = Regex.Replace(processed, @"\/\*\s*{\s*(?<annotation>.*)\s*}\s*\*\/", m => m.Value.Replace(",", CommaReplacement));

            // Removes breaklines and tabs:
            processed = Regex.Replace(processed, "\r|\n|\t", string.Empty);

            // Adds ';' to the end
            processed = processed.Trim() + ';';

            return processed;
        }

        private string PostProcessAnnotationText(string sqlText)
        {
            return sqlText.Replace(CommaReplacement, ",");
        }

        private void ParseTableAnnotations(string sqlText, SqlAnnotationsCollection<string> results)
        {
            Regex regex = new Regex(@"CREATE\s+TABLE\s+(?<tbName>\w+)\s*(?<annotations>\/\*\s*{(?<innerAnnotations>.*?)}\s*\*\/)?[\s\n]*\((?<columnsList>.*?)(\)\s*(;|GO))", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var matches = regex.Matches(sqlText);

            foreach (Match match in matches)
            {
                var tableName = match.Groups["tbName"].Value.Trim();
                var annotation = match.Groups["innerAnnotations"].Value.Trim();
                var columnsList = match.Groups["columnsList"].Value.Trim();

                if (!string.IsNullOrWhiteSpace(annotation))
                {
                    var processedAnnotation = PostProcessAnnotationText(annotation);
                    results.AddTableAnnotation(tableName, processedAnnotation);
                }
                ParseColumnAnnotations(tableName, columnsList, results);
            }
        }

        private void ParseColumnAnnotations(string tableName, string columnsList, SqlAnnotationsCollection<string> results)
        {
            Regex annotationsRegex = new Regex(@"(?<=,)*?(?<columnName>\w+){1}.*\/\*\s*{\s*(?<innerAnnotations>.*)\s*}\s*\*\/", RegexOptions.IgnoreCase);

            var columns = columnsList.Split(',');

            foreach (var column in columns)
            {
                var annotationsMatches = annotationsRegex.Matches(column);
                if (annotationsMatches.Count > 0)
                {
                    var columnName = annotationsMatches[0].Groups["columnName"].Value.Trim();
                    var annotation = annotationsMatches[0].Groups["innerAnnotations"].Value.Trim();

                    if (!string.IsNullOrWhiteSpace(annotation))
                    {
                        var processedAnnotation = PostProcessAnnotationText(annotation);
                        results.AddColumnAnnotation(tableName, columnName, processedAnnotation);
                    }
                }
            }

        }
    }
}
