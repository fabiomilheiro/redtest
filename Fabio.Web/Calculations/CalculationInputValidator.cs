using System;
using Fabio.Web.Domain;

namespace Fabio.Web.Calculations
{
    public interface ICalculationInputValidator
    {
        void Validate(CalculationInput input);
    }

    public class CalculationInputValidator : ICalculationInputValidator
    {
        public void Validate(CalculationInput input)
        {
            ValidateProbability(input.A, nameof(input.A));
            ValidateProbability(input.B, nameof(input.B));
        }

        private static void ValidateProbability(double probability, string parameterName)
        {
            if (probability < 0 || probability > 1)
            {
                throw new ArgumentException(parameterName);
            }
        }
    }
}