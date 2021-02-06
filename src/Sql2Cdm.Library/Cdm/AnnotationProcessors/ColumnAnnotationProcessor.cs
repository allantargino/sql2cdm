using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models.Annotations;

namespace Sql2Cdm.Library.Cdm.AnnotationProcessors
{
    public class ColumnAnnotationProcessor : IAnnotationProcessorVisitor
    {
        private readonly CdmCorpusDefinition corpus;
        private readonly CdmReferenceResolver resolver;
        private readonly CdmTypeAttributeDefinition attribute;

        public ColumnAnnotationProcessor(CdmCorpusDefinition corpus, CdmReferenceResolver resolver, CdmTypeAttributeDefinition attribute)
        {
            this.corpus = corpus;
            this.resolver = resolver;
            this.attribute = attribute;
        }

        public void Process(DefaultAnnotation annotation) { }

        public void Process(ExtendsAnnotation annotation) { }

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

            attribute.AppliedTraits.Add(trait);
        }

        public void Process(DisplayNameAnnotation annotation)
        {
            attribute.DisplayName = annotation.Value;
        }

        public void Process(DescriptionAnnotation annotation)
        {
            attribute.Description = annotation.Value;
        }
    }
}
