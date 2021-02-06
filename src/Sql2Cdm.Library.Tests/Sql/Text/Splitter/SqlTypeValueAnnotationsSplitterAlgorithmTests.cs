using Sql2Cdm.Library.Sql.Text.Splitter;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Text.Splitter
{
    public class SqlTypeValueAnnotationsSplitterAlgorithmTests
    {
        [Theory]
        [InlineData("type:something", "type", "something")]
        [InlineData("type: something", "type", "something")]
        [InlineData("type :something", "type", "something")]
        [InlineData("type : something", "type", "something")]
        [InlineData("type:some thing", "type", "some thing")]
        [InlineData("type : some thing", "type", "some thing")]
        [InlineData("TRAIT : some thing", "TRAIT", "some thing")]
        [InlineData("type:SOMEthing", "type", "SOMEthing")]
        [InlineData("type:means.address.main", "type", "means.address.main")]
        [InlineData("type:", "type", "")]
        [InlineData("type", "type", "")]
        public void SplitsValidAnnotationPatterns(string textAnnotation, string expectedType, string expectedValue)
        {
            var (actualType, actualValue) = SqlTypeValueAnnotationsSplitterAlgorithm.SplitTypeValueFromAnnotation(textAnnotation);

            Assert.Equal(expectedType, actualType);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(":")]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowsExceptionWithInvalidAnnotation(string textAnnotation)
        {
            void action() => SqlTypeValueAnnotationsSplitterAlgorithm.SplitTypeValueFromAnnotation(textAnnotation);

            Assert.Throws<SqlAnnotationSplitException>(action);
        }
    }
}
