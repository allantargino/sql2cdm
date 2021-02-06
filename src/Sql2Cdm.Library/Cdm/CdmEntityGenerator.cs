using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Sql2Cdm.Library.Cdm.AnnotationProcessors;
using Sql2Cdm.Library.Models;

namespace Sql2Cdm.Library.Cdm
{
    public class CdmEntityGenerator
    {
        private readonly CdmCorpusDefinition corpus;
        private readonly CdmReferenceResolver resolver;
        private readonly CdmDocumentDefinition entityDocument;
        private readonly string version;

        public CdmEntityGenerator(CdmCorpusDefinition corpus, CdmReferenceResolver resolver, CdmDocumentDefinition entityDocument, string version)
        {
            this.corpus = corpus;
            this.resolver = resolver;
            this.entityDocument = entityDocument;
            this.version = version;
        }

        public CdmEntityDefinition GenerateEntity(Table table)
        {
            CdmEntityDefinition entity = entityDocument.Definitions.Add(CdmObjectType.EntityDef, table.Name) as CdmEntityDefinition;

            MapTableColumnsToCdmAttributes(table, entity);

            ProcessTableAnnotations(table, entity);

            AddVersioning(entity);

            return entity;
        }

        private void MapTableColumnsToCdmAttributes(Table table, CdmEntityDefinition entity)
        {
            foreach (Column column in table.Columns)
            {
                CdmTypeAttributeDefinition attribute = new CdmTypeAttributeDefinition(corpus.Ctx, column.Name);

                string attributeType = CdmSqlTypeMapper.Map(column.Type).ToString();
                attribute.DataType = new CdmDataTypeReference(corpus.Ctx, attributeType, true);

                attribute.IsNullable = column.IsNullable;

                if (column.IsPrimaryKey)
                {
                    attribute.Purpose = new CdmPurposeReference(corpus.Ctx, "identifiedBy", true);
                }

                if (column.Length.MaxSize > 0)
                {
                    attribute.MaximumLength = column.Length.MaxSize;
                }

                ProcessColumnAnnotations(column, attribute);

                entity.Attributes.Add(attribute);
            }
        }

        private void ProcessColumnAnnotations(Column column, CdmTypeAttributeDefinition attribute)
        {
            var columnProcessor = new ColumnAnnotationProcessor(corpus, resolver, attribute);

            foreach (var annotation in column.Annotations)
            {
                annotation.Process(columnProcessor);
            }
        }

        private void ProcessTableAnnotations(Table table, CdmEntityDefinition entityDefinition)
        {
            var tableProcessor = new TableAnnotationProcessor(corpus, resolver, entityDocument, entityDefinition);
            
            foreach (var annotation in table.Annotations)
            {
                annotation.Process(tableProcessor);
            }
        }

        private void AddVersioning(CdmEntityDefinition entity)
        {
            if (!string.IsNullOrWhiteSpace(version))
            {
                entity.Version = version;

                var traitVersion = entity.ExhibitsTraits.Add("is.CDM.entityVersion", simpleRef: false);
                traitVersion.Arguments.Add("versionNumber", version);
            }
        }
    }
}
