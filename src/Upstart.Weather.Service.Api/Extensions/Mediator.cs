using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Upstart.Weather.Service.Application.QueryHandlers;
using Upstart.Weather.Service.Domain.Commons;
using Upstart.Weather.Service.Domain.Geocoding.Queries;

namespace Upstart.Weather.Service.Api.Extensions
{
    public static class Mediator
    {
        public static void AddMediator(this IServiceCollection serviceCollection)
        {
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(GetGeocodingDataByAddressQuery).Assembly,
                typeof(GeocodingQueryHandler).Assembly,
            };

            serviceCollection.AddMediatR(assemblies);

            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehaviour<,>));
        }
    }
}
