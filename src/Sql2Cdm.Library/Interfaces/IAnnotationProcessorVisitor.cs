using Sql2Cdm.Library.Models.Annotations;

namespace Sql2Cdm.Library.Interfaces
{
    public interface IAnnotationProcessorVisitor
    {
        void Process(DefaultAnnotation annotation);
        void Process(DescriptionAnnotation annotation);
        void Process(DisplayNameAnnotation annotation);
        void Process(ExtendsAnnotation annotation);
        void Process(TraitAnnotation annotation);
    }
}
