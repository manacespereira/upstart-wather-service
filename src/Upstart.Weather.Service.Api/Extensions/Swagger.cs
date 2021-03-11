using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Upstart.Weather.Service.Api.Extensions
{
    public static class Swagger
    {
        public static void AddSwagger(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"API | {configuration["Swagger:Title"]}",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "engineering@upstartweather.com",
                        Name = "Upstart 13 - Software Engineering"
                    },
                    Description = $"{configuration["Swagger:Description"]}"
                });
            });
        }

        public static void UseSwagger(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"API - {configuration["Swagger:Title"]}");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }
    }
}
