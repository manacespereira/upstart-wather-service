using System;
using System.Collections.Generic;
using Upstart.Weather.Service.Domain.Weather.Models;

namespace Upstart.Weather.Service.Application.Responses
{
    public class GetWeatherByAddressResponse
    {
        public GetWeatherByAddressResponse(IEnumerable<WeatherPeriod> periods, ElevationArea elevation)
        {
            Periods = periods;
            Elevation = elevation;
        }

        public IEnumerable<WeatherPeriod> Periods { get; set; }
        public ElevationArea Elevation { get; set; }
    }
}
