using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Upstart.Weather.Service.Application.Policies;
using Upstart.Weather.Service.Infra.ExternalServices;

namespace Upstart.Weather.Service.Api.Extensions
{
    public static class ExternalServices
    {
        public static void AddGeocodingApi(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddRefitClient<IGeocodingService>()
                .ConfigureHttpClient(options =>
                {
                    options.BaseAddress = new Uri(configuration["GeocodingApi:BaseUrl"]);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                .AddPolicyHandler(JittererRetryPolicy.GetPolicy());
        }

        public static void AddWeatherApi(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddRefitClient<IWeatherService>()
                            .ConfigureHttpClient(options =>
                            {
                                options.BaseAddress = new Uri(configuration["WeatherApi:BaseUrl"]);
                            })
                            .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                            .AddPolicyHandler(JittererRetryPolicy.GetPolicy());
        }
    }
}