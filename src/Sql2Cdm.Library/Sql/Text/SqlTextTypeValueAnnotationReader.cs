using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Sql.Annotations.DataStructures;
using Sql2Cdm.Library.Sql.Annotations.Parser;
using Sql2Cdm.Library.Sql.Text.Splitter;

namespace Sql2Cdm.Library.Sql.Text
{
    public class SqlTextTypeValueAnnotationReader : ITypeValueAnnotationsReader
    {
        private readonly SqlTextScriptReader textScriptReader;
        private readonly SqlTextAnnotationsParser textAnnotationParser;
        private readonly SqlMultipleAnnotationsSplitter multipleAnnotationsSplitter;
        private readonly SqlTypeValueAnnotationsSplitter typeValueAnnotationsSplitter;

        public SqlTextTypeValueAnnotationReader(SqlTextScriptReader sqlTextScriptReader)
        {
            this.textScriptReader = sqlTextScriptReader;
            this.textAnnotationParser = new SqlTextAnnotationsParser();
            this.multipleAnnotationsSplitter = new SqlMultipleAnnotationsSplitter();
            this.typeValueAnnotationsSplitter = new SqlTypeValueAnnotationsSplitter();
        }

        public SqlAnnotationsCollection<SqlTypeValueAnnotation> ReadTypeValueAnnotations()
        {
            string sqlText = textScriptReader.ReadAllText();

            SqlAnnotationsCollection<string> multipleTextAnnotations = textAnnotationParser.ParseTextAnnotations(sqlText);

            SqlAnnotationsCollection<string> singleTextAnnotations = multipleAnnotationsSplitter.Map(multipleTextAnnotations);

            return typeValueAnnotationsSplitter.Map(singleTextAnnotations);
        }
    }
}
