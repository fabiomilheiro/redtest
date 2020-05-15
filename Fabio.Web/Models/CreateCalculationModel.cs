using System.ComponentModel.DataAnnotations;
using Fabio.Web.ModelValidations;

namespace Fabio.Web.Models
{
    public class CreateCalculationModel
    {
        /// <summary>
        /// The probability A.
        /// </summary>
        /// <example>0.5</example>
        [Required]
        [Range(0, 1)]
        public double? A { get; set; }
        
        /// <summary>
        /// The probability B.
        /// </summary>
        /// <example>0.6</example>
        [Required]
        [Range(0, 1)]
        public double? B { get; set; }
        
        /// <summary>
        /// The calculation type.
        /// </summary>
        /// <example>1</example>
        [Required]
        [CalculationTypeValidation]
        public int? CalculationType { get; set; }
    }
}