using System.IO.Abstractions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fabio.Web.Domain;
using Fabio.Web.Models;
using Fabio.Web.ServiceTests.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Fabio.Web.ServiceTests
{
    public class CalculationTests
    {
        private readonly TestApplicationFactory factory;
        private readonly HttpClient client;

        public CalculationTests()
        {
            this.factory = new TestApplicationFactory();
            this.client = this.factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.Replace(new ServiceDescriptor(typeof(IFileSystem), this.factory.FileSystem));
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task Calculate_InvalidA_ReturnsBadRequest()
        {
            var response = await this.client.PostJsonAsync("/calculations", new CreateCalculationModel
            {
                A = 300,
                B = 1,
                CalculationType = 1
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.ShouldHaveError("A", "The field A must be between 0 and 1.");
        }

        [Fact]
        public async Task Calculate_InvalidB_ReturnsBadRequest()
        {
            var response = await this.client.PostJsonAsync("/calculations", new CreateCalculationModel
            {
                A = 0,
                B = 2,
                CalculationType = 1
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.ShouldHaveError("B", "The field B must be between 0 and 1.");
        }

        [Fact]
        public async Task Calculate_InvalidCalculationType_ReturnsBadRequest()
        {
            var response = await this.client.PostJsonAsync("/calculations", new CreateCalculationModel
            {
                A = 0,
                B = 0,
                CalculationType = 2000
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.ShouldHaveError("CalculationType", "The CalculationType specified is not valid.");
        }

        [Fact]
        public async Task Calculate_ValidRequest_ReturnsResult()
        {
            var response = await this.client.PostJsonAsync("/calculations", new CreateCalculationModel
            {
                A = 0.5,
                B = 0.6,
                CalculationType = (int)CalculationType.Combined
            });

            response.ShouldHaveStatusCode(HttpStatusCode.OK);
            response.ShouldHaveBody(new CalculationResultModel
            {
                Value = 0.3
            });
        }

        [Fact]
        public async Task Calculate_ValidRequest_SavesToFile()
        {
            var response = await this.client.PostJsonAsync("/calculations", new CreateCalculationModel
            {
                A = 0.5,
                B = 0.6,
                CalculationType = (int)CalculationType.Combined
            });

            response.ShouldHaveStatusCode(HttpStatusCode.OK);
            this.factory.FileSystem.File
                .ReadAllText(TestApplicationFactory.AuditsPath)
                .Should()
                .NotBeNullOrEmpty();
        }
    }
}