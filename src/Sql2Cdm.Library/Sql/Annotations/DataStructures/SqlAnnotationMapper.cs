using System.Linq;

namespace Sql2Cdm.Library.Sql.Annotations.DataStructures
{
    public abstract class SqlAnnotationMapper<TFrom, TTo>
    {
        public SqlAnnotationsCollection<TTo> Map(SqlAnnotationsCollection<TFrom> holderFrom)
        {
            var holderTo = new SqlAnnotationsCollection<TTo>();

            foreach (var tableResult in holderFrom.TableAnnotationResults)
            {
                TTo to = Map(tableResult.Result);
                holderTo.AddTableAnnotation(tableResult.TableName, to);
            }

            foreach (var columnResult in holderFrom.ColumnAnnotationResults)
            {
                TTo to = Map(columnResult.Result);
                holderTo.AddColumnAnnotation(columnResult.TableName, columnResult.ColumnName, to);
            }

            return holderTo;
        }

        protected abstract TTo Map(TFrom annotation);
    }
}
