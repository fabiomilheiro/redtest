using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabio.Web.Calculations;
using Fabio.Web.Domain;
using Fabio.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fabio.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationsController : ControllerBase
    {
        private readonly ICalculationService calculationService;

        public CalculationsController(ICalculationService calculationService)
        {
            this.calculationService = calculationService;
        }

        [HttpPost]
        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        public async Task<ActionResult<CalculationResultModel>> Post([FromBody]CreateCalculationModel model)
        {
            var result = await this.calculationService
                .ExecuteAsync(
                    (CalculationType)model.CalculationType.Value,
                    new CalculationInput
                    {
                        A = model.A.Value,
                        B = model.B.Value
                    });

            return Ok(new CalculationResultModel
            {
                Value = result.Value
            });
        }
    }
}