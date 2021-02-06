using System.Collections.Generic;
using System.Linq;

namespace Sql2Cdm.Library.Sql.Text.Splitter
{
    public static class SqlMultipleAnnotationsSplitterAlgorithm
    {
        public static IEnumerable<string> SplitMultipleAnnotations(string multipleAnnotationsText)
        {
            return multipleAnnotationsText
                    .Split(SqlTextAnnotationConstants.ANNOTATION_MULTIPLE_SEPARATION_CHAR_VALUE)
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Select(a => a.Trim());
        }
    }
}
