using Fabio.Web.Calculations;
using Fabio.Web.Domain;
using FluentAssertions;
using Xunit;

namespace Fabio.Web.UnitTests.Calculations
{
    public class EitherCalculationTests
    {
        private readonly EitherCalculation sut;

        public EitherCalculationTests()
        {
            this.sut = new EitherCalculation();
        }

        [Fact]
        public void Execute_Always_ReturnsProbabilityOfEither()
        {
            var result = this.sut.Execute(new CalculationInput
            {
                A = 0.5,
                B = 0.6
            });

            result.Should().BeEquivalentTo(
                new CalculationResult
                {
                    Value = 0.8
                });
        }
    }
}