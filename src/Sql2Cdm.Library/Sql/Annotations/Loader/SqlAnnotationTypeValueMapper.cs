using Sql2Cdm.Library.Models.Annotations;
using Sql2Cdm.Library.Sql.Annotations.DataStructures;

namespace Sql2Cdm.Library.Sql.Annotations.Loader
{
    public class SqlAnnotationTypeValueMapper : SqlAnnotationMapper<SqlTypeValueAnnotation, Annotation>
    {
        private readonly SqlAnnotationAssemblyLoader<Annotation, DefaultAnnotation> annotationLoader;

        public SqlAnnotationTypeValueMapper()
        {
            annotationLoader = new SqlAnnotationAssemblyLoader<Annotation, DefaultAnnotation>();
        }

        protected override Annotation Map(SqlTypeValueAnnotation sqlTypeValueAnnotation)
        {
            return annotationLoader.LoadAnnotation(sqlTypeValueAnnotation.Type, sqlTypeValueAnnotation.Value);
        }
    }
}