using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Sql2Cdm.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sql2Cdm.Library.Cdm
{
    public class CdmEntityRelationshipGenerator
    {
        private readonly CdmCorpusDefinition corpus;
        private readonly CdmReferenceResolver resolver;

        public CdmEntityRelationshipGenerator(CdmCorpusDefinition corpus, CdmReferenceResolver resolver)
        {
            this.corpus = corpus;
            this.resolver = resolver;
        }

        public IEnumerable<CdmE2ERelationship> GenerateRelationships(Table table)
        {
            var fkColumns = table.Columns.Where(t => t.IsForeignKey);

            foreach (var column in fkColumns)
            {
                var relationshipName = $"relationship-{table.Name}-{column.Name}";
                var relationship = corpus.MakeObject<CdmE2ERelationship>(CdmObjectType.E2ERelationshipDef, relationshipName);

                string fromDocumentName = resolver.GetDocumentFileName(table.Name);
                relationship.FromEntity = $"{fromDocumentName}/{table.Name}";
                relationship.FromEntityAttribute = column.Name;

                string toDocumentName = resolver.GetDocumentFileName(column.ForeignKey.Table.Name);
                relationship.ToEntity = $"{toDocumentName}/{column.ForeignKey.Table.Name}";
                relationship.ToEntityAttribute = column.ForeignKey.Name;

                yield return relationship;
            }
        }
    }
}