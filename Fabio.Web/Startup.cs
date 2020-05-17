using System;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Text.RegularExpressions;
using Fabio.Web.Calculations;
using Fabio.Web.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Fabio.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(AddCorsPolicy);

            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<ICalculationFactory, CalculationFactory>();
            services.AddScoped<ICalculationInputValidator, CalculationInputValidator>();
            services.AddScoped<ICalculationAuditRepository, CalculationAuditRepository>();
            services.AddScoped<IFileSystem, FileSystem>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Calculations Web API",
                        Version = "v1"
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("Default");

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculations Web API V1");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddCorsPolicy(CorsOptions options)
        {
            options.AddPolicy("Default", builder =>
            {
                if (Environment.IsDevelopment())
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
                else
                {
                    builder
                        .SetIsOriginAllowed(origin => Regex.IsMatch(origin, "^https://fabio-redington.azurewebsites.net$"))
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            });
        }
    }
}
