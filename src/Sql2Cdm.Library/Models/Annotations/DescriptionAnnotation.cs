using Sql2Cdm.Library.Interfaces;

namespace Sql2Cdm.Library.Models.Annotations
{
    public sealed class DescriptionAnnotation : Annotation
    {
        public DescriptionAnnotation(string value) : base(value)
        {
        }

        public override void Process(IAnnotationProcessorVisitor processor)
        {
            processor.Process(this);
        }
    }
}
