using System.Collections.Generic;

namespace Sql2Cdm.Library.Sql.Annotations.DataStructures
{
    public abstract class SqlAnnotationFlatMapper<TFrom, TTo>
    {
        public SqlAnnotationsCollection<TTo> Map(SqlAnnotationsCollection<TFrom> holderFrom)
        {
            var holderTo = new SqlAnnotationsCollection<TTo>();

            foreach (var tableResult in holderFrom.TableAnnotationResults)
            {
                IEnumerable<TTo> toExpanded = Map(tableResult.Result);
                foreach (TTo to in toExpanded)
                    holderTo.AddTableAnnotation(tableResult.TableName, to);
            }

            foreach (var columnResult in holderFrom.ColumnAnnotationResults)
            {
                IEnumerable<TTo> toExpanded = Map(columnResult.Result);
                foreach (TTo to in toExpanded)
                    holderTo.AddColumnAnnotation(columnResult.TableName, columnResult.ColumnName, to);
            }

            return holderTo;
        }

        protected abstract IEnumerable<TTo> Map(TFrom annotation);
    }
}
