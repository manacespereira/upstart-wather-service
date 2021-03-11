using System.Collections.Generic;

namespace Upstart.Weather.Service.Domain.Weather.Models
{
    public class WeatherProperties
    {
        public string GridId { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        public IEnumerable<WeatherPeriod> Periods { get; set; }
        public ElevationArea Elevation { get; set; }
    }
}