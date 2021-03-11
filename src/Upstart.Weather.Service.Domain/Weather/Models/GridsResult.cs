﻿
namespace Upstart.Weather.Service.Domain.Weather.Models
{
    public class GridsResult
    {
        public GridsResult(string id, WeatherProperties properties)
        {
            Id = id;
            Properties = properties;
        }

        public string Id { get; set; }
        public WeatherProperties Properties { get; set; }
    }
}
