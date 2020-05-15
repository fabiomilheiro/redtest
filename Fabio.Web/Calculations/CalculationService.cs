using System;
using System.Threading.Tasks;
using Fabio.Web.DataAccess;
using Fabio.Web.Domain;

namespace Fabio.Web.Calculations
{
    public interface ICalculationService
    {
        Task<CalculationResult> ExecuteAsync(CalculationType type, CalculationInput input);
    }

    public class CalculationService : ICalculationService
    {
        private readonly ICalculationInputValidator calculationInputValidator;
        private readonly ICalculationFactory calculationFactory;
        private readonly ICalculationAuditRepository calculationAuditRepository;

        public CalculationService(
            ICalculationInputValidator calculationInputValidator,
            ICalculationFactory calculationFactory,
            ICalculationAuditRepository calculationAuditRepository)
        {
            this.calculationInputValidator = calculationInputValidator;
            this.calculationFactory = calculationFactory;
            this.calculationAuditRepository = calculationAuditRepository;
        }

        public async Task<CalculationResult> ExecuteAsync(CalculationType type, CalculationInput input)
        {
            this.calculationInputValidator.Validate(input);

            var result = this.calculationFactory.Create(type).Execute(input);

            await this.calculationAuditRepository.SaveAsync(new CalculationAudit
            {
                CalculationType = type,
                Input = input,
                Result = result,
                Timestamp = DateTime.UtcNow
            });

            return result;
        }
    }
}