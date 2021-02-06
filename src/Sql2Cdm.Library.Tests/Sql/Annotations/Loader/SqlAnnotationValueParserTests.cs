using Sql2Cdm.Library.Sql.Annotations.Loader;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Annotations.Loader
{
    public class SqlAnnotationValueParserTests
    {
        [Theory]
        [InlineData("is.constrained.by", "is.constrained.by")]
        [InlineData("is.constrained.by", "is.constrained.by()")]
        [InlineData("is.constrained.by", "is.constrained.by(min=100)")]
        [InlineData("is.constrained.by", "is.constrained.by( min=100)")]
        [InlineData("is.constrained.by", "is.constrained.by( min=100 )")]
        [InlineData("is.constrained.by", "is.constrained.by(min=100 )")]
        [InlineData("is.constrained.by", "is.constrained.by(min=100,max=200)")]
        [InlineData("is.constrained.by", "is.constrained.by(min=100, max=200)")]
        [InlineData("is.constrained.by", "is.constrained.by(min=100 ,max=200)")]
        [InlineData("is.constrained.by", "is.constrained.by(100)")]
        [InlineData("is.constrained.by", "is.constrained.by(100,200)")]
        [InlineData("is.constrained.by", "is.constrained.by(100, 200)")]
        [InlineData("is.constrained.by", "is.constrained.by(100 ,200)")]
        [InlineData("is.constrained.by", "is.constrained.by( 100 , 200 )")]
        [InlineData("is.constrained.by", "is.constrained.by(pattern=(abc.*))")]
        public void ParseValue(string expectedValue, string annotationValue)
        {
            var parser = new SqlAnnotationValueParser(annotationValue);

            var actualValue = parser.ParseValue();

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("is.constrained.by")]
        [InlineData("is.constrained.by()")]
        [InlineData("is.constrained.by(min=100)", "min", "100")]
        [InlineData("is.constrained.by(min=100,)", "min", "100")]
        [InlineData("is.constrained.by(min=100, )", "min", "100")]
        [InlineData("is.constrained.by(min=100 , )", "min", "100")]
        [InlineData("is.constrained.by( min=100)", "min", "100")]
        [InlineData("is.constrained.by( min=100 )", "min", "100")]
        [InlineData("is.constrained.by(min=100 )", "min", "100")]
        [InlineData("is.constrained.by(min=100,max=200)", "min", "100", "max", "200")]
        [InlineData("is.constrained.by(min=100, max=200)", "min", "100", "max", "200")]
        [InlineData("is.constrained.by(min=100 ,max=200)", "min", "100", "max", "200")]
        [InlineData("is.constrained.by(100,max=200)", "", "100", "max", "200")]
        [InlineData("is.constrained.by(min=100,200)", "min", "100", "", "200")]
        [InlineData("is.constrained.by(abc)", "", "abc")]
        [InlineData("is.constrained.by(abc, size=100)", "", "abc", "size", "100")]
        [InlineData("is.constrained.by(100)", "", "100")]
        [InlineData("is.constrained.by(100,200)", "", "100", "", "200")]
        [InlineData("is.constrained.by(100, 200)", "", "100", "", "200")]
        [InlineData("is.constrained.by(100 ,200)", "", "100", "", "200")]
        [InlineData("is.constrained.by( 100 , 200 )", "", "100", "", "200")]
        [InlineData("is.constrained.by(pattern=(abc.*))", "pattern", "(abc.*)")]
        [InlineData("is.constrained.by(min_pattern=(abc.*), max_pattern=(def.*))", "min_pattern", "(abc.*)", "max_pattern", "(def.*)")]
        public void ParseArguments(string annotationValue, params string[] expectedParsedArguments)
        {
            IEnumerable<KeyValuePair<string, dynamic>> expectedArguments = BuildExpectedParsedArguments(expectedParsedArguments);
            var parser = new SqlAnnotationValueParser(annotationValue);

            IEnumerable<KeyValuePair<string, dynamic>> actualArguments = parser.ParseValueArguments();

            Assert.Equal(expectedArguments, actualArguments);
        }

        private IEnumerable<KeyValuePair<string, dynamic>> BuildExpectedParsedArguments(string[] expectedParsedArguments)
        {
            if (expectedParsedArguments.Length == 0)
                yield break;

            if (expectedParsedArguments.Length % 2 == 1)
                throw new Exception("args must be key/value pairs");

            for (int i = 0; i < expectedParsedArguments.Length; i+=2)
            {
                string key = expectedParsedArguments[i];
                string value = expectedParsedArguments[i+1];
                yield return new KeyValuePair<string, dynamic>(key, value);
            }
        }
    }
}
