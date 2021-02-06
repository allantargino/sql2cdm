using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Models.Annotations;
using Xunit;

namespace Sql2Cdm.Library.Tests.Models
{
    public class TableTests
    {
        [Fact]
        public void AnnotationsAreInitializedEmpty()
        {
            var table = new Table("name");

            Assert.Empty(table.Annotations);
        }

        [Fact]
        public void EqualAnnotationsAreInsertedJustOnce()
        {
            var table = new Table("name");
            
            var annotation1 = new TraitAnnotation("something");
            var annotation2 = new TraitAnnotation("something");

            table.Annotations.Add(annotation1);
            table.Annotations.Add(annotation2);

            Assert.Single(table.Annotations);
        }
    }
}
