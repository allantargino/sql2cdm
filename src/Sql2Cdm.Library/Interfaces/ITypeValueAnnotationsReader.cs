using Sql2Cdm.Library.Sql.Annotations.DataStructures;

namespace Sql2Cdm.Library.Interfaces
{
    public interface ITypeValueAnnotationsReader
    {
        SqlAnnotationsCollection<SqlTypeValueAnnotation> ReadTypeValueAnnotations();
    }
}
