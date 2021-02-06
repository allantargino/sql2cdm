namespace Sql2Cdm.Library.Models
{
    public record ColumnLength
    {
        public int MaxSize { get; init; }
        public int Precision { get; init; }
        public int Digits { get; init; }
    }
}
