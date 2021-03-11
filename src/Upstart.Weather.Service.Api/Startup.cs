using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Network;
using Upstart.Weather.Service.Api.Extensions;

namespace Upstart.Weather.Service.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddRepositories();

            services.AddMediator();

            services.AddSwagger(Configuration);

            services.AddHealthChecks();

            services.AddCors();

            services.AddGeocodingApi(Configuration);

            services.AddWeatherApi(Configuration);

            services.AddMemoryCache();

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", Configuration.GetValue<string>("Serilog:applicationName"))
                .Enrich.WithExceptionDetails()
                .WriteTo.UDPSink(Configuration.GetValue<string>("Serilog:remoteAddress"),
                    Configuration.GetValue<int>("Serilog:remotePort"))
                .MinimumLevel.Is(Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(e =>
            {
                e.AllowAnyOrigin();
                e.AllowAnyMethod();
                e.AllowAnyHeader();
            });

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseSwagger(Configuration);

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
