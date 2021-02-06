using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Models.Annotations;
using System;
using Xunit;

namespace Sql2Cdm.Library.Tests.Models
{
    public class AnnotationTests
    {
        [Fact]
        public void ArgumentsAreInitializedEmpty()
        {
            var annotation = CreateAnnotation();

            Assert.Empty(annotation.Arguments);
        }

        [Theory]
        [InlineData("value")]
        [InlineData(3)]
        [InlineData(3.14)]
        public void AddArgumentValueWithoutKey(dynamic value)
        {
            var annotation = CreateAnnotation();

            annotation.AddArgument(string.Empty, value);

            Assert.Single(annotation.Arguments);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CannotAddArgumentValueWithoutValue(dynamic value)
        {
            var annotation = CreateAnnotation();

            void action() => annotation.AddArgument(string.Empty, value);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Theory]
        [InlineData("key", "value")]
        [InlineData("key", 3)]
        [InlineData("key", 3.14)]
        public void ArgumentsAreInsertedJustOnce(string key, dynamic value)
        {
            var annotation = CreateAnnotation();

            annotation.AddArgument(key, value);
            annotation.AddArgument(key, value);

            Assert.Single(annotation.Arguments);
        }

        private Annotation CreateAnnotation()
        {
            return new TraitAnnotation("value");
        }
    }
}
