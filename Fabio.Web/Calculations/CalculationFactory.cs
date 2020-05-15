using System;
using Fabio.Web.Domain;

namespace Fabio.Web.Calculations
{
    public interface ICalculationFactory
    {
        ICalculation Create(CalculationType type);
    }

    public class CalculationFactory : ICalculationFactory
    {
        public ICalculation Create(CalculationType type)
        {
            return type switch
            {
                CalculationType.Either => new EitherCalculation(),
                CalculationType.Combined => new CombinedCalculation(),
                _ => throw new ArgumentException("Must be a valid calculation type.", nameof(type))
            };
        }
    }
}