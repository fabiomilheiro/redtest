using Fabio.Web.Domain;

namespace Fabio.Web.Calculations
{
    public class CombinedCalculation : ICalculation
    {
        public CalculationResult Execute(CalculationInput input)
        {
            return new CalculationResult
            {
                Value = input.A * input.B
            };
        }
    }
}