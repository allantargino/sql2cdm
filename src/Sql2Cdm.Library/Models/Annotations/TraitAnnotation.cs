using Sql2Cdm.Library.Interfaces;
using System.Collections.Generic;

namespace Sql2Cdm.Library.Models.Annotations
{
    public sealed class TraitAnnotation : Annotation
    {
        public TraitAnnotation(string value) : base(value)
        {
        }

        public TraitAnnotation(string value, IEnumerable<KeyValuePair<string, dynamic>> arguments)
            : base(value, arguments)
        {
        }

        public override void Process(IAnnotationProcessorVisitor processor)
        {
            processor.Process(this);
        }
    }
}
