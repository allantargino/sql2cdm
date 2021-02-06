namespace Sql2Cdm.Library.Sql.Annotations.DataStructures
{
    public record SqlTableAnnotationResult<TAnnotation>
    {
        public string TableName { get; init; }
        public TAnnotation Result { get; init; }
    }
}
