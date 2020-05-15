using System;
using Fabio.Web.Calculations;
using Fabio.Web.Domain;
using FluentAssertions;
using Xunit;

namespace Fabio.Web.UnitTests.Calculations
{
    public class CalculationFactoryTests
    {
        private readonly CalculationFactory sut;

        public CalculationFactoryTests()
        {
            this.sut = new CalculationFactory();
        }

        [Fact]
        public void Create_TypeNotValid_Throws()
        {
            Action action = () => this.sut.Create((CalculationType)10000);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Create_TypeCombined_ReturnsCombinedCalculation()
        {
            var result = this.sut.Create(CalculationType.Combined);

            result.Should().BeOfType<CombinedCalculation>();
        }

        [Fact]
        public void Create_TypeEither_ReturnsEitherCalculation()
        {
            var result = this.sut.Create(CalculationType.Combined);

            result.Should().BeOfType<CombinedCalculation>();
        }
    }
}