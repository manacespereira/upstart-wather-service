using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Refit;
using Upstart.Weather.Service.Application.Responses;
using Upstart.Weather.Service.Domain.Commons;
using Upstart.Weather.Service.Domain.Geocoding.Queries;
using Upstart.Weather.Service.Infra.ExternalServices;

namespace Upstart.Weather.Service.Application.QueryHandlers
{
    public class GeocodingQueryHandler : IRequestHandler<GetGeocodingDataByAddressQuery, IResult>
    {
        private readonly IGeocodingService geocodingService;
        public GeocodingQueryHandler(IGeocodingService geocodingService)
        {
            this.geocodingService = geocodingService;
        }

        public async Task<IResult> Handle(GetGeocodingDataByAddressQuery request, CancellationToken cancellationToken)
        {
            var geocodingResult = await geocodingService.GetGeocodingDataFromAddressAsync(request.Address);
            var result = geocodingResult?.Result;

            if (result != null && !result.AddressMatches.Any())
                return Result.Ok();

            var address = result.AddressMatches.FirstOrDefault();

            var response = new GetGeocodingDataByAddressResponse(address.Coordinates.X, address.Coordinates.Y);

            return Result.Ok(response);
        }
    }
}