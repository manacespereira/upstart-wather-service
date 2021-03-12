﻿using System;
namespace Upstart.Weather.Service.UnitTests.Geocoding
{
    public static class GeocodingFaker
    {
        public const string JsonResponseOK = "{ \"result\": {\r\n    \"input\": {\r\n      \"benchmark\": {\r\n        \"id\": \"2020\",\r\n        \"benchmarkName\": \"Public_AR_Census2020\",\r\n        \"benchmarkDescription\": \"Public Address Ranges - Census 2020 Benchmark\",\r\n        \"isDefault\": false\r\n      },\r\n      \"address\": {\r\n        \"address\": \"1631 Pleasant Plains Rd Matthews, NC 28105\"\r\n      }\r\n    },\r\n    \"addressMatches\": [\r\n      {\r\n        \"matchedAddress\": \"1631 PLEASANT PLAINS RD, MATTHEWS, NC, 28105\",\r\n        \"coordinates\": {\r\n          \"x\": -80.72488,\r\n          \"y\": 35.098015\r\n        },\r\n        \"tigerLine\": {\r\n          \"tigerLineId\": \"70539029\",\r\n          \"side\": \"R\"\r\n        },\r\n        \"addressComponents\": {\r\n          \"fromAddress\": \"1699\",\r\n          \"toAddress\": \"1601\",\r\n          \"preQualifier\": \"\",\r\n          \"preDirection\": \"\",\r\n          \"preType\": \"\",\r\n          \"streetName\": \"PLEASANT PLAINS\",\r\n          \"suffixType\": \"RD\",\r\n          \"suffixDirection\": \"\",\r\n          \"suffixQualifier\": \"\",\r\n          \"city\": \"MATTHEWS\",\r\n          \"state\": \"NC\",\r\n          \"zip\": \"28105\"\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}";
        public const string JsonResponseNoContent = "{ \"result\": {\r\n    \"input\": {\r\n      \"benchmark\": {\r\n        \"id\": \"2020\",\r\n        \"benchmarkName\": \"Public_AR_Census2020\",\r\n        \"benchmarkDescription\": \"Public Address Ranges - Census 2020 Benchmark\",\r\n        \"isDefault\": false\r\n      },\r\n      \"address\": {\r\n        \"address\": \"1631 Pleasant Plains Rd Matthews, NC 28105\"\r\n      }\r\n    },\r\n    \"addressMatches\": []\r\n  }\r\n}";
    }
}