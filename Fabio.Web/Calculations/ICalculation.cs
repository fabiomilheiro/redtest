using Fabio.Web.Domain;

namespace Fabio.Web.Calculations
{
    public interface ICalculation
    {
        CalculationResult Execute(CalculationInput input);
    }
}