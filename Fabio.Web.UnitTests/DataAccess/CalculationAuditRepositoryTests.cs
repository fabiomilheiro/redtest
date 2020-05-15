using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Fabio.Web.DataAccess;
using Fabio.Web.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Fabio.Web.UnitTests.DataAccess
{
    public class CalculationAuditRepositoryTests
    {
        private static readonly string AuditsPath = $@"C:\app\{CalculationAuditRepository.AuditsFilename}";

        private readonly CalculationAuditRepository sut;
        private readonly MockFileSystem fileSystem;
        private readonly Mock<IWebHostEnvironment> environment;

        public CalculationAuditRepositoryTests()
        {
            this.fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {AuditsPath, new MockFileData("")}
            });
            this.environment = new Mock<IWebHostEnvironment>();
            this.environment.SetupGet(e => e.ContentRootPath).Returns(@"C:\app\");

            this.sut = new CalculationAuditRepository(this.fileSystem, environment.Object);
        }

        [Fact]
        public void SaveAsync_Null_Throws()
        {
            Func<Task> func = async () => await this.sut.SaveAsync(null);

            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task SaveAsync_FileDoesNotExist_CreatesNewFileAndEntry()
        {
            fileSystem.File.Delete(AuditsPath);
            var audit = new CalculationAudit
            {
                CalculationType = CalculationType.Either,
                Input = new CalculationInput
                {
                    A = 0.1,
                    B = 0.2
                },
                Timestamp = new DateTime(2000, 1, 2),
                Result = new CalculationResult
                {
                    Value = 1
                }

            };

            await this.sut.SaveAsync(audit);

            var contents = this.fileSystem.File.ReadAllText(AuditsPath).Trim().Split(Environment.NewLine);

            contents.Should().HaveCount(1);
            contents.Should().BeEquivalentTo(JsonSerializer.Serialize(audit));
        }

        [Fact]
        public async Task SaveAsync_RequestValid_AppendsAudit()
        {
            var audit = new CalculationAudit
            {
                CalculationType = CalculationType.Either,
                Input = new CalculationInput
                {
                    A = 0.1,
                    B = 0.2
                },
                Timestamp = new DateTime(2000, 1, 2),
                Result = new CalculationResult
                {
                    Value = 1
                }

            };

            await this.sut.SaveAsync(audit);
            await this.sut.SaveAsync(audit);

            var contents = this.fileSystem.File.ReadAllText(AuditsPath).Trim().Split(Environment.NewLine);

            contents.Should().HaveCount(2);
            contents.Should().BeEquivalentTo(
                $"{JsonSerializer.Serialize(audit)}",
                $"{JsonSerializer.Serialize(audit)}");
        }
    }
}