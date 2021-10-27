using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TransferMarktScraper.WebApi.Services;
using TransferMarktScraper.WebApi.Services.Interfaces;
using TransferMarktScraper.Core;
using Serilog;

namespace TransferMarktScraper.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<DbConfig>(Configuration.GetSection("environmentVariables"));
            services.RegisterServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransferMarktScraper.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Errors");
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransferMarktScraper.WebApi v1"));
            }
            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbContext, DbContext>();

            services.AddTransient<ITeamServices, TeamServices>();
            services.AddTransient<IPlayerServices, PlayerServices>();
            services.AddTransient<IMarketValueServices, MarketValueServices>();
            services.AddTransient<IPerformanceServices, PerformanceServices>();
            return services;
        }
    }
}
