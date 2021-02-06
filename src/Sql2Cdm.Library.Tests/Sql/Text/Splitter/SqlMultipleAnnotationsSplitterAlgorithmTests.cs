using Sql2Cdm.Library.Sql.Text.Splitter;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Text.Splitter
{
    public class SqlMultipleAnnotationsSplitterAlgorithmTests
    {
        [Theory]
        [InlineData("", new string[] { })]
        [InlineData("trait", new string[] { "trait" })]
        [InlineData("trait ", new string[] { "trait" })]
        [InlineData(" trait ", new string[] { "trait" })]
        [InlineData("trait:", new string[] { "trait:" })]
        [InlineData("trait: ", new string[] { "trait:" })]
        [InlineData(" trait: ", new string[] { "trait:" })]
        [InlineData(" trait:", new string[] { "trait:" })]
        [InlineData("trait:means", new[] { "trait:means" })]
        [InlineData("trait:means ", new[] { "trait:means" })]
        [InlineData(" trait:means ", new[] { "trait:means" })]
        [InlineData(" trait:means", new[] { "trait:means" })]
        [InlineData("trait:means;", new[] { "trait:means" })]
        [InlineData("trait:means; ", new[] { "trait:means" })]
        [InlineData("trait:means ;", new[] { "trait:means" })]
        [InlineData("trait:means ; ", new[] { "trait:means" })]
        [InlineData("trait:means;trait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait:means; trait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait:means ; trait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait:means ;trait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait:means;\ntrait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait:means\n;trait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait:means\n;\ntrait:is.dataFormat", new[] { "trait:means", "trait:is.dataFormat" })]
        [InlineData("trait : means;trait : is.dataFormat", new[] { "trait : means", "trait : is.dataFormat" })]
        public void ProcessValidAnnotations(string multipleAnnotationsText, string[] expectedSingleAnnotations)
        {
            var actualSingleAnnotations = SqlMultipleAnnotationsSplitterAlgorithm.SplitMultipleAnnotations(multipleAnnotationsText);

            Assert.Equal(expectedSingleAnnotations, actualSingleAnnotations);
        }
    }
}
