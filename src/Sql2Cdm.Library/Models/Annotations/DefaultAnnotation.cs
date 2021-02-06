using Sql2Cdm.Library.Interfaces;

namespace Sql2Cdm.Library.Models.Annotations
{
    public sealed class DefaultAnnotation : Annotation
    {
        public DefaultAnnotation(string value) : base(value)
        {
        }

        public override void Process(IAnnotationProcessorVisitor processor)
        {
            processor.Process(this);
        }
    }
}
