using System.ComponentModel.DataAnnotations;
using Fabio.Web.ModelValidations;

namespace Fabio.Web.Models
{
    public class CreateCalculationModel
    {
        [Required]
        [Range(0, 1)]
        public double? A { get; set; }
        
        [Required]
        [Range(0, 1)]
        public double? B { get; set; }
        
        [Required]
        [CalculationTypeValidation]
        public int? CalculationType { get; set; }
    }
}