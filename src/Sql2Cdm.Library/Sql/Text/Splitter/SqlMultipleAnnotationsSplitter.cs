using Sql2Cdm.Library.Sql.Annotations.DataStructures;
using System.Collections.Generic;

namespace Sql2Cdm.Library.Sql.Text.Splitter
{
    public class SqlMultipleAnnotationsSplitter : SqlAnnotationFlatMapper<string, string>
    {
        protected override IEnumerable<string> Map(string multipleTextAnnotation)
        {
            return SqlMultipleAnnotationsSplitterAlgorithm.SplitMultipleAnnotations(multipleTextAnnotation);
        }
    }
}
