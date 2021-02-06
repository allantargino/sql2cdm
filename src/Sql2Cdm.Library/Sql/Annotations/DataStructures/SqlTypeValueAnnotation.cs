namespace Sql2Cdm.Library.Sql.Annotations.DataStructures
{
    public record SqlTypeValueAnnotation
    {
        public string Type { get; init; }
        public string Value { get; init; }
    }
}
