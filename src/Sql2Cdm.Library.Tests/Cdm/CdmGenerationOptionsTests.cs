using Sql2Cdm.Library.Cdm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sql2Cdm.Library.Tests.Cdm
{
    public class CdmGenerationOptionsTests
    {
        [Fact]
        public void DefaultOptionsAreValid()
        {
            var sut = new CdmGenerationOptions();
            var context = new ValidationContext(sut);
            var errors = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(sut, context, errors, validateAllProperties: true);

            Assert.Empty(errors);
            Assert.True(isValid);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1.2")]
        [InlineData("1.2.3")]
        [InlineData("1.0.0")]
        [InlineData("10.20.30")]
        [InlineData("100.200.300")]
        [Theory]
        public void OptionEntitiesVersioningIsValid(string version)
        {
            var sut = new CdmGenerationOptions() { EntitiesVersion = version };
            var context = new ValidationContext(sut);
            var errors = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(sut, context, errors, validateAllProperties: true);

            Assert.Empty(errors);
            Assert.True(isValid);
        }

        [InlineData(" ")]
        [InlineData("1.")]
        [InlineData("1.2.")]
        [InlineData("1.2.3.")]
        [InlineData("1.2.3.4")]
        [InlineData("a.2.3")]
        [InlineData("1.b.3")]
        [InlineData("1.2.c")]
        [Theory]
        public void OptionEntitiesVersioningIsInvalid(string version)
        {
            var sut = new CdmGenerationOptions() { EntitiesVersion = version };
            var context = new ValidationContext(sut);
            var errors = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(sut, context, errors, validateAllProperties: true);

            Assert.NotEmpty(errors);
            Assert.False(isValid);
        }
    }
}
