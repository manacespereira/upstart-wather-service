﻿using System;
namespace Upstart.Weather.Service.Application.Responses
{
    public class GetGeocodingDataByAddressResponse
    {
        public GetGeocodingDataByAddressResponse(double x, double y)
        {
            Lat = y;
            Lng = x;
        }

        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
