using System;
using System.Linq;

namespace Sql2Cdm.Library.Sql.Text.Splitter
{
    public static class SqlTypeValueAnnotationsSplitterAlgorithm
    {
        public static (string type, string value) SplitTypeValueFromAnnotation(string textAnnotation)
        {
            if (string.IsNullOrWhiteSpace(textAnnotation))
                throw new SqlAnnotationSplitException(textAnnotation);

            var processed = textAnnotation.Trim();

            try
            {
                if (processed.Contains(SqlTextAnnotationConstants.ANNOTATION_SPLIT_CHAR_VALUE.ToString()))
                {
                    var (left, right) = Split(processed);

                    if (string.IsNullOrWhiteSpace(left) && string.IsNullOrWhiteSpace(right))
                        throw new SqlAnnotationSplitException(processed);

                    return (left, right);
                }
                else
                {
                    return (processed, string.Empty);
                }
            }
            catch (ArgumentException inner)
            {
                throw new SqlAnnotationSplitException(processed, inner);
            }
        }

        private static (string left, string right) Split(string textAnnotation)
        {
            string[] split = textAnnotation
                                .Split(new[] { SqlTextAnnotationConstants.ANNOTATION_SPLIT_CHAR_VALUE }, count: 2)
                                .Select(s => s.Trim())
                                .ToArray();

            return (split[0], split[1]);
        }
    }
}
