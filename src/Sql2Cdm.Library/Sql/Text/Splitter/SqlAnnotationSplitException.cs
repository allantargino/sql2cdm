using System;

namespace Sql2Cdm.Library.Sql.Text.Splitter
{
    public class SqlAnnotationSplitException : Exception
    {
        public string Annotation { get; }

        public SqlAnnotationSplitException(string annotation) : this(annotation, null) { }

        public SqlAnnotationSplitException(string annotation, Exception innerException)
            : base($"The following annotation is not valid: {annotation}", innerException)
        {
            Annotation = annotation;
        }
    }
}
