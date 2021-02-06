using Sql2Cdm.Library.Interfaces;

namespace Sql2Cdm.Library.Models.Annotations
{
    public sealed class ExtendsAnnotation : Annotation
    {
        public ExtendsAnnotation(string value) : base(value)
        {
        }

        public override void Process(IAnnotationProcessorVisitor processor)
        {
            processor.Process(this);
        }
    }
}
