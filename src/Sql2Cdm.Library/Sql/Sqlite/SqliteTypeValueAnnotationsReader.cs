using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Sql.Annotations.DataStructures;

namespace Sql2Cdm.Library.Sql.Sqlite
{
    public class SqliteTypeValueAnnotationsReader : ITypeValueAnnotationsReader
    {
        public SqlAnnotationsCollection<SqlTypeValueAnnotation> ReadTypeValueAnnotations()
        {
            return new SqlAnnotationsCollection<SqlTypeValueAnnotation>();
        }
    }
}
