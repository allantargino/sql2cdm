using Sql2Cdm.Library.Sql.Annotations.DataStructures;

namespace Sql2Cdm.Library.Sql.Text.Splitter
{
    public class SqlTypeValueAnnotationsSplitter : SqlAnnotationMapper<string, SqlTypeValueAnnotation>
    {
        protected override SqlTypeValueAnnotation Map(string textAnnotation)
        {
            var (type, value) = SqlTypeValueAnnotationsSplitterAlgorithm.SplitTypeValueFromAnnotation(textAnnotation);
            return new SqlTypeValueAnnotation() { Type = type, Value = value };
        }
    }
}
