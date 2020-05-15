using System;
using System.IO;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Fabio.Web.DataAccess
{
    public interface ICalculationAuditRepository
    {
        Task SaveAsync(CalculationAudit audit);
    }

    public class CalculationAuditRepository : ICalculationAuditRepository
    {
        public const string AuditsFilename = "calculation-audits.txt";
        private readonly IFileSystem fileSystem;
        private readonly IWebHostEnvironment environment;
        private readonly string auditsPath;

        public CalculationAuditRepository(IFileSystem fileSystem, IWebHostEnvironment environment)
        {
            this.fileSystem = fileSystem;
            this.environment = environment;
            this.auditsPath = Path.Combine(environment.ContentRootPath, AuditsFilename);
        }

        public async Task SaveAsync(CalculationAudit audit)
        {
            if (audit == null)
            {
                throw new ArgumentNullException(nameof(audit));
            }

            await this.fileSystem.File.AppendAllTextAsync(
                auditsPath,
                $"{JsonSerializer.Serialize(audit)}{Environment.NewLine}");
        }
    }
}