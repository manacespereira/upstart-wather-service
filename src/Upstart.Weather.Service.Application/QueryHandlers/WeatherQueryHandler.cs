using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Upstart.Weather.Service.Application.Responses;
using Upstart.Weather.Service.Domain.Commons;
using Upstart.Weather.Service.Domain.Geocoding.Queries;
using Upstart.Weather.Service.Domain.Weather.Models;
using Upstart.Weather.Service.Domain.Weather.Queries;
using Upstart.Weather.Service.Infra.ExternalServices;

using System.Linq;
namespace Upstart.Weather.Service.Application.QueryHandlers
{
    public class WeatherQueryHandler : IRequestHandler<GetWeatherByAddressQuery, IResult>
    {
        private readonly IWeatherService weatherService;
        private readonly IMediator mediator;
        private readonly IMemoryCache memoryCache;

        public WeatherQueryHandler(IWeatherService weatherService, IMediator mediator, IMemoryCache memoryCache)
        {
            this.weatherService = weatherService;
            this.mediator = mediator;
            this.memoryCache = memoryCache;
        }

        public async Task<IResult> Handle(GetWeatherByAddressQuery request, CancellationToken cancellationToken)
        {
            GetWeatherByAddressResponse response;
            memoryCache.TryGetValue<GetWeatherByAddressResponse>($"{request.Address.ToLower()}-{request.NumberOfDays}", out response);

            if (response != null) return Result.Ok(response);

            var coordinatesResponse = await mediator.Send(new GetGeocodingDataByAddressQuery(request.Address));

            if (coordinatesResponse == null || !coordinatesResponse.HasValue)
            {
                request.AddNotification("Error", "We could not get informations about the informed address");
                return Result.BadRequest(request.Notifications);
            }

            var coordinates = coordinatesResponse.GetObjectValue<GetGeocodingDataByAddressResponse>();

            if (coordinates == null) return Result.BadRequest();

            var grids = await weatherService.GetGridsByLatLngAsync(coordinates.Lat, coordinates.Lng);

            var weatherResult = await weatherService.GetWeatherByGridsAsync(grids.Properties.GridId, grids.Properties.GridX, grids.Properties.GridY);

            var weatherProperties = weatherResult.Properties;

            var periodsByNumberOfDays = weatherProperties.Periods.Take(request.NumberOfDays * 2);

            response = new GetWeatherByAddressResponse(periodsByNumberOfDays, weatherProperties.Elevation);

            memoryCache.Set<GetWeatherByAddressResponse>(request.Address.ToLower(), response, TimeSpan.FromMinutes(5));

            return Result.Ok(response);
        }
    }
}
