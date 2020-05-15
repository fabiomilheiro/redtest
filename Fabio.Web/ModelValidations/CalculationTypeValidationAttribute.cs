using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fabio.Web.Domain;

namespace Fabio.Web.ModelValidations
{
    public class CalculationTypeValidationAttribute : ValidationAttribute
    {
        private static readonly int[] AvailableCalculationTypes = Enum.GetValues(typeof(CalculationType)).Cast<int>().ToArray();
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var calculationType = value as int?;

            if (calculationType == null)
            {
                return ValidationResult.Success;
            }

            if (!AvailableCalculationTypes.Contains(calculationType.Value))
            {
                return new ValidationResult(
                    $"The {validationContext.MemberName} specified is not valid.{Environment.NewLine}" +
                    $"Valid values: {GetAvailableValues()}");
            }

            return ValidationResult.Success;
        }

        private static string GetAvailableValues()
        {
            return string.Join(
                ", ",
                AvailableCalculationTypes
                    .Select(GetCalculationTypeFriendlyName));
        }

        private static string GetCalculationTypeFriendlyName(int value)
        {
            return $"{value} ({(CalculationType) value})";
        }
    }
}