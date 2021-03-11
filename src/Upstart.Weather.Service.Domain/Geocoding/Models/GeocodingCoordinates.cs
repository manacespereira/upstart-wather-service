using System;
using System.Text.Json.Serialization;

namespace Upstart.Weather.Service.Domain.Geocoding.Models
{
    public class GeocodingCoordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
