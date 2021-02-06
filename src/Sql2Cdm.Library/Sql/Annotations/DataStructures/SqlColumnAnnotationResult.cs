namespace Sql2Cdm.Library.Sql.Annotations.DataStructures
{
    public record SqlColumnAnnotationResult<TAnnotation>
    {
        public string TableName { get; init; }
        public string ColumnName { get; init; }
        public TAnnotation Result { get; init; }
    }
}
