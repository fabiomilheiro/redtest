using System;
using Fabio.Web.Calculations;
using Fabio.Web.Domain;
using FluentAssertions;
using Xunit;

namespace Fabio.Web.UnitTests.Calculations
{
    public class CalculationInputValidatorTests
    {
        private readonly CalculationInputValidator sut;

        public CalculationInputValidatorTests()
        {
            this.sut = new CalculationInputValidator();
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        public void Validate_InvalidA_Throws(double invalidProbability)
        {
            Action action = () => this.sut.Validate(new CalculationInput
            {
                A = invalidProbability,
                B = 0.5
            });

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        public void Validate_InvalidB_Throws(double invalidProbability)
        {
            Action action = () => this.sut.Validate(new CalculationInput
            {
                A = 0.5,
                B = invalidProbability
            });

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Validate_IsValid_DoesNothing()
        {
            Action action = () => this.sut.Validate(new CalculationInput
            {
                A = 0.5,
                B = 0.5
            });

            action.Should().NotThrow();
        }
    }
}