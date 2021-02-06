using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sql2Cdm.Library.Sql.Annotations.DataStructures
{
    public sealed class SqlAnnotationsCollection<TAnnotation>
    {
        private readonly List<SqlTableAnnotationResult<TAnnotation>> tableAnnotationResults;
        private readonly List<SqlColumnAnnotationResult<TAnnotation>> columnAnnotationResults;

        public IEnumerable<SqlTableAnnotationResult<TAnnotation>> TableAnnotationResults => tableAnnotationResults.ToImmutableList();
        public IEnumerable<SqlColumnAnnotationResult<TAnnotation>> ColumnAnnotationResults => columnAnnotationResults.ToImmutableList();

        public SqlAnnotationsCollection()
        {
            tableAnnotationResults = new List<SqlTableAnnotationResult<TAnnotation>>();
            columnAnnotationResults = new List<SqlColumnAnnotationResult<TAnnotation>>();
        }

        public void AddTableAnnotation(string tableName, TAnnotation annotation)
        {
            tableAnnotationResults.Add(
                new SqlTableAnnotationResult<TAnnotation>()
                {
                    TableName = tableName,
                    Result = annotation
                });
        }

        public void AddColumnAnnotation(string tableName, string columnName, TAnnotation annotation)
        {
            columnAnnotationResults.Add(
                new SqlColumnAnnotationResult<TAnnotation>
                {
                    TableName = tableName,
                    ColumnName = columnName,
                    Result = annotation
                });
        }
    }
}
