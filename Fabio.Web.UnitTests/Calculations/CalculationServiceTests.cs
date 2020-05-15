using System;
using System.Threading.Tasks;
using Fabio.Web.Calculations;
using Fabio.Web.DataAccess;
using Fabio.Web.Domain;
using FluentAssertions;
using Moq;
using Xunit;

namespace Fabio.Web.UnitTests.Calculations
{
    public class CalculationServiceTests
    {
        private readonly CalculationService sut;
        private readonly Mock<ICalculation> calculation;
        private readonly Mock<ICalculationFactory> calculationFactoryMock;
        private readonly Mock<ICalculationInputValidator> inputValidatorMock;
        private readonly Mock<ICalculationAuditRepository> auditRepository;
        private CalculationAudit savedAudit;

        public CalculationServiceTests()
        {
            this.calculation = new Mock<ICalculation>();
            this.calculation
                .Setup(c => c.Execute(It.IsAny<CalculationInput>()))
                .Returns(new CalculationResult());

            this.calculationFactoryMock = new Mock<ICalculationFactory>();
            this.calculationFactoryMock
                .Setup(f => f.Create(CalculationType.Either))
                .Returns(this.calculation.Object);

            this.inputValidatorMock = new Mock<ICalculationInputValidator>();

            this.auditRepository = new Mock<ICalculationAuditRepository>();
            this.auditRepository
                .Setup(r => r.SaveAsync(It.IsAny<CalculationAudit>()))
                .Callback<CalculationAudit>(audit => this.savedAudit = audit);

            this.sut = new CalculationService(
                this.inputValidatorMock.Object,
                this.calculationFactoryMock.Object,
                this.auditRepository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Always_Validates()
        {
            var input = new CalculationInput();

            var result = await this.sut.ExecuteAsync(CalculationType.Either, input);

            this.inputValidatorMock.Verify(v => v.Validate(input));
        }

        [Fact]
        public async Task ExecuteAsync_Always_ReturnsTheCalculationResult()
        {
            var input = new CalculationInput
            {
                A = 0,
                B = 0
            };
            this.calculation
                .Setup(c => c.Execute(input))
                .Returns(new CalculationResult
                {
                    Value = 1
                });

            var result = await this.sut.ExecuteAsync(CalculationType.Either, input);

            result.Should().BeEquivalentTo(
                new CalculationResult
                {
                    Value = 1
                });
        }

        [Fact]
        public async Task ExecuteAsync_Always_Audits()
        {
            var input = new CalculationInput
            {
                A = 0.1,
                B = 0.1
            };
            var calculationResult = new CalculationResult
            {
                Value = 1
            };
            this.calculation
                .Setup(c => c.Execute(input))
                .Returns(calculationResult);

            var result = await this.sut.ExecuteAsync(CalculationType.Either, input);

            this.savedAudit.Should().NotBeNull();
            this.savedAudit.CalculationType.Should().Be(CalculationType.Either);
            this.savedAudit.Input.Should().BeEquivalentTo(input);
            this.savedAudit.Result.Should().BeEquivalentTo(result);
            this.savedAudit.Timestamp.Should().BeCloseTo(DateTime.UtcNow);
        }
    }
}