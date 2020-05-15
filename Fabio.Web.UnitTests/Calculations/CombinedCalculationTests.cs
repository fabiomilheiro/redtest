using Fabio.Web.Calculations;
using Fabio.Web.Domain;
using FluentAssertions;
using Xunit;

namespace Fabio.Web.UnitTests.Calculations
{
    public class CombinedCalculationTests
    {
        private CombinedCalculation sut;

        public CombinedCalculationTests()
        {
            this.sut = new CombinedCalculation();
        }

        [Fact]
        public void Execute_Always_ReturnsCombinedProbability()
        {
            var result = this.sut.Execute(new CalculationInput
            {
                A = 0.6,
                B = 0.5
            });

            result.Should().BeEquivalentTo(
                new CalculationResult
                {
                    Value = 0.3
                });
        }
    }
}