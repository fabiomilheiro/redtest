using System.ComponentModel.DataAnnotations;
using Fabio.Web.Domain;
using Fabio.Web.Models;
using Fabio.Web.ModelValidations;
using FluentAssertions;
using Xunit;

namespace Fabio.Web.UnitTests.ModelValidations
{
    public class CalculationTypeValidationAttributeTests
    {
        private readonly CalculationTypeValidationAttribute sut;

        public CalculationTypeValidationAttributeTests()
        {
            this.sut = new CalculationTypeValidationAttribute();
        }

        [Fact]
        public void IsValid_ValueNull_ReturnsSuccess()
        {
            var result = this.sut.GetValidationResult(
                null,
                new ValidationContext(new CreateCalculationModel()));

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void IsValid_ValueNotNullableInteger_ReturnsSuccess()
        {
            var result = this.sut.GetValidationResult(
                "Some value of the wrong type",
                new ValidationContext(new CreateCalculationModel()));

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void IsValid_ValueDoesNotMatchAnyEnumValue_ReturnsError()
        {
            var result = this.sut.GetValidationResult(
                50000,
                new ValidationContext(new CreateCalculationModel())
                {
                    MemberName = "fieldName"
                });

            result.Should().BeEquivalentTo(
                new ValidationResult(
                    "The fieldName specified is not valid.\r\n" +
                    "Valid values: 0 (Combined), 1 (Either)"));
        }

        [Theory]
        [InlineData(CalculationType.Combined)]
        [InlineData(CalculationType.Either)]
        public void IsValid_ValueMatchesEnumValue_ReturnsSuccess(CalculationType calculationType)
        {
            var result = this.sut.GetValidationResult(
                calculationType,
                new ValidationContext(new CreateCalculationModel()));

            result.Should().Be(ValidationResult.Success);
        }
    }
}