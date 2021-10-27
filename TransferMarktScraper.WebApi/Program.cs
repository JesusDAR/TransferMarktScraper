using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransferMarktScraper.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "Development")
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithEnvironmentName()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("Logs\\log.txt")
                    .CreateLogger();
            }
            if (environment == "Production")
            {
                string connectionStringLogs = Environment.GetEnvironmentVariable("CONNECTION_STRING_LOGS");
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithEnvironmentName()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.AzureBlobStorage(connectionStringLogs)
                    .CreateLogger();
            }
            try
            {
                Log.Information("Aplication Starting Up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
