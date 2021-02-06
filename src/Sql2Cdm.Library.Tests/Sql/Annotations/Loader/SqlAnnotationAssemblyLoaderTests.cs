using Sql2Cdm.Library.Models.Annotations;
using Sql2Cdm.Library.Sql.Annotations.Loader;
using System;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Annotations.Loader
{
    public class SqlAnnotationAssemblyLoaderTests
    {
        [Theory]
        [InlineData("anything", "")]
        [InlineData("anything", "something")]
        public void ConvertsToDefaultAnnotationWhenNotRecognized(string type, string value)
        {
            var loader = CreateLoader();

            Annotation actual = loader.LoadAnnotation(type, value);

            Assert.NotNull(actual);
            Assert.IsType<DefaultAnnotation>(actual);
        }

        [Theory]
        [InlineData("trait", "", typeof(TraitAnnotation))]
        [InlineData("trait", "value", typeof(TraitAnnotation))]
        [InlineData("extends", "", typeof(ExtendsAnnotation))]
        [InlineData("extends", "value", typeof(ExtendsAnnotation))]
        public void ConvertsToCorrectType(string type, string expectedValue, Type expectedType)
        {
            var loader = CreateLoader();

            Annotation actual = loader.LoadAnnotation(type, expectedValue);

            Assert.IsType(expectedType, actual);
            Assert.Equal(expectedValue, actual.Value);
        }

        private SqlAnnotationAssemblyLoader<Annotation, DefaultAnnotation> CreateLoader()
        {
            return new SqlAnnotationAssemblyLoader<Annotation, DefaultAnnotation>();
        }
    }
}
