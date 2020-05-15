using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabio.Web.Calculations;
using Fabio.Web.Domain;
using Fabio.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fabio.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CalculationsController : ControllerBase
    {
        private readonly ICalculationService calculationService;

        public CalculationsController(ICalculationService calculationService)
        {
            this.calculationService = calculationService;
        }

        /// <summary>
        /// Creates a new calculation.
        /// </summary>
        /// <param name="model">The calculation parameters.</param>
        /// <response code="200">Returns the calculation result.</response>
        /// <response code="400">Returns the validation errors.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CalculationResultModel), StatusCodes.Status200OK)]
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