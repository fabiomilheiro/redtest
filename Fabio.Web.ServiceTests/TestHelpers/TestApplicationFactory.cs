using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
using Fabio.Web.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Fabio.Web.ServiceTests.TestHelpers
{
    public class TestApplicationFactory : WebApplicationFactory<Startup>
    {
        public static readonly string AuditsPath =
            Path.Combine(
                GetContentDirectory(),
                CalculationAuditRepository.AuditsFilename);

        public const string BaseUrl = "http://localhost:5555";

        public TestApplicationFactory()
        {
            this.FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { AuditsPath, new MockFileData("") }
            });
        }

        public MockFileSystem FileSystem { get; set; }

        protected override void ConfigureClient(HttpClient client)
        {
            client.BaseAddress = new Uri(BaseUrl);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseKestrel()
                .CaptureStartupErrors(false)
                .UseStartup<Startup>()
                .UseUrls(BaseUrl);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static string GetContentDirectory()
        {
            return Directory.CreateDirectory(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.Replace(".ServiceTests", null);
        }
    }
}