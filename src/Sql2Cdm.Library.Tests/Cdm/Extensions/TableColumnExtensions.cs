using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Models.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Sql2Cdm.Library.Tests.Cdm.Extensions
{
    public static class TableColumnExtensions
    {
        public static Table WithColumn(this Table table, string columnName, SqlDbType? type, bool isNullable = false, bool isPrimaryKey = false, Column foreignKey = null, Annotation annotation = null)
        {
            var newColumn = new Column(columnName, table)
            {
                Type = type,
                IsNullable = isNullable,
                IsPrimaryKey = isPrimaryKey,
                ForeignKey = foreignKey,
                Annotations = annotation != null ? new HashSet<Annotation>() { annotation } : new HashSet<Annotation>()
            };

            var newColumns = new List<Column>() { newColumn };

            if (table.Columns.Count() > 0)
            {
                newColumns.AddRange(table.Columns);
            }
            table.Columns = newColumns;

            return table;
        }

        public static Table WithForeignColumn(this Table table, string columnName, SqlDbType type, Table foreignTable, string foreignColumnName)
        {
            var foreignColumn = foreignTable.Columns.First(c => c.Name == foreignColumnName);

            table.WithColumn(columnName, type, foreignKey: foreignColumn);

            return table;
        }

        public static Table WithAnnotation(this Table table, Annotation annotation)
        {
            var newAnnotations = new HashSet<Annotation>() { annotation };

            if (table.Annotations.Count() > 0)
            {
                foreach (var item in table.Annotations)
                {
                    newAnnotations.Add(item);
                }
            }
            table.Annotations = newAnnotations;

            return table;
        }

        public static Column GetColumn(this Table table)
        {
            return table.Columns.First();
        }
    }
}
