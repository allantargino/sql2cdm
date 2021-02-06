using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Sql.Annotations.Loader;
using Sql2Cdm.Library.Sql.Annotations.DataStructures;
using Sql2Cdm.Library.Sql.Annotations.Alias;
using System.Linq;
using Sql2Cdm.Library.Models.Annotations;

namespace Sql2Cdm.Library.Sql.Annotations.Combiner
{
    public class SqlRelationalModelAnnotationCombiner
    {
        private readonly ITypeValueAnnotationsReader annotationsReader;
        private readonly SqlAnnotationTypeAliasMapper aliasMapper;
        private readonly SqlAnnotationTypeValueMapper typeValueMapper;

        public SqlRelationalModelAnnotationCombiner(ITypeValueAnnotationsReader annotationsReader, SqlAnnotationTypeAliasMapper aliasMapper)
        {
            this.annotationsReader = annotationsReader;
            this.aliasMapper = aliasMapper;
            this.typeValueMapper = new SqlAnnotationTypeValueMapper();
        }

        public void ReadAnnotationsAndCombineWithModel(RelationalModel model)
        {
            SqlAnnotationsCollection<SqlTypeValueAnnotation> typeValueAnnotations = annotationsReader.ReadTypeValueAnnotations();

            SqlAnnotationsCollection<SqlTypeValueAnnotation> aliasedAnnotations = aliasMapper.Map(typeValueAnnotations);
            
            SqlAnnotationsCollection<Annotation> annotations = typeValueMapper.Map(aliasedAnnotations);

            Combine(model, annotations);
        }

        private void Combine(RelationalModel model, SqlAnnotationsCollection<Annotation> annotations)
        {
            foreach (var table in model.Tables)
            {
                var tableAnnotations = annotations.TableAnnotationResults
                                                        .Where(a => a.TableName == table.Name)
                                                        .Select(a => a.Result);

                foreach (var tableAnnotation in tableAnnotations)
                {
                    table.Annotations.Add(tableAnnotation);
                }

                foreach (var column in table.Columns)
                {
                    var columnAnnotations = annotations.ColumnAnnotationResults
                                                            .Where(a => a.TableName == table.Name && a.ColumnName == column.Name)
                                                            .Select(a => a.Result);

                    foreach (var columnAnnotation in columnAnnotations)
                    {
                        column.Annotations.Add(columnAnnotation);
                    }
                }
            }
        }
    }
}
