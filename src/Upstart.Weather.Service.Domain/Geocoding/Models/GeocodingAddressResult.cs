using System.Collections;
using System.Collections.Generic;

namespace Upstart.Weather.Service.Domain.Geocoding.Models
{
    public class GeocodingAddressResult
    {
        public IEnumerable<GeocodingAddress> AddressMatches { get; set; }
    }
}