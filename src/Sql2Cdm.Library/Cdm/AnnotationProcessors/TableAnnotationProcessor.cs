using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models.Annotations;

namespace Sql2Cdm.Library.Cdm.AnnotationProcessors
{
    public class TableAnnotationProcessor: IAnnotationProcessorVisitor
    {
        private readonly CdmCorpusDefinition corpus;
        private readonly CdmReferenceResolver resolver;
        private readonly CdmDocumentDefinition entityDocument;
        private readonly CdmEntityDefinition entityDefinition;

        public TableAnnotationProcessor(CdmCorpusDefinition corpus, CdmReferenceResolver resolver, CdmDocumentDefinition entityDocument, CdmEntityDefinition entityDefinition)
        {
            this.corpus = corpus;
            this.resolver = resolver;
            this.entityDocument = entityDocument;
            this.entityDefinition = entityDefinition;
        }

        public void Process(DefaultAnnotation annotation) { }

        public void Process(TraitAnnotation annotation)
        {
            string traitName = annotation.Value;
            CdmTraitReference trait = corpus.MakeObject<CdmTraitReference>(CdmObjectType.TraitRef, traitName, false);

            foreach (var argument in annotation.Arguments)
            {
                if (argument.Value != null)
                {
                    trait.Arguments.Add(argument.Key, argument.Value);
                }
            }

            entityDefinition.ExhibitsTraits.Add(trait);
        }

        public void Process(ExtendsAnnotation annotation)
        {
            var extendedEntity = annotation.Value;
            string documentName = resolver.GetDocumentFileName(extendedEntity);

            entityDocument.Imports.Add(documentName);
            entityDefinition.ExtendsEntity = new CdmEntityReference(entityDefinition.Ctx, extendedEntity, true);
        }

        public void Process(DisplayNameAnnotation annotation)
        {
            entityDefinition.DisplayName = annotation.Value;
        }

        public void Process(DescriptionAnnotation annotation)
        {
            entityDefinition.Description = annotation.Value;
        }
    }
}
