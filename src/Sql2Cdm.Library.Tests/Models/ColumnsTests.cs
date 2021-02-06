using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Models.Annotations;
using System.Collections.Generic;
using Xunit;

namespace Sql2Cdm.Library.Tests.Models
{
    public class ColumnsTests
    {
        [Fact]
        public void AnnotationsAreInitializedEmpty()
        {
            var column = new Column("name", null);

            Assert.Empty(column.Annotations);
        }

        [Fact]
        public void EqualAnnotationsAreInsertedJustOnce()
        {
            var args1 = new List<KeyValuePair<string, dynamic>>();
            args1.Add(new KeyValuePair<string, dynamic>("key", "value"));
            Annotation annotation12 = new TraitAnnotation("value", args1);

            var args2 = new List<KeyValuePair<string, dynamic>>();
            args2.Add(new KeyValuePair<string, dynamic>("key", "value"));
            Annotation annotation22 = new TraitAnnotation("value", args2);

            var res = annotation12 == annotation22;


            var column = new Column("name", null);

            Annotation annotation1 = new TraitAnnotation("value");
            Annotation annotation2 = new TraitAnnotation("value");

            column.Annotations.Add(annotation1);
            column.Annotations.Add(annotation2);

            Assert.Single(column.Annotations);
        }
    }
}
