using Fabio.Web.Domain;

namespace Fabio.Web.Calculations
{
    public class EitherCalculation : ICalculation
    {
        public CalculationResult Execute(CalculationInput input)
        {
            return new CalculationResult
            {
                Value = SumProbabilities(input) - GetCombinedProbability(input)
            };
        }

        private static double SumProbabilities(CalculationInput input)
        {
            return input.A + input.B;
        }

        private static double GetCombinedProbability(CalculationInput input)
        {
            return input.A * input.B;
        }
    }
}