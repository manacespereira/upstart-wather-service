using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Upstart.Weather.Service.Application.QueryHandlers;
using Upstart.Weather.Service.Application.Responses;
using Upstart.Weather.Service.Domain.Geocoding.Models;
using Upstart.Weather.Service.Domain.Geocoding.Queries;
using Upstart.Weather.Service.Infra.ExternalServices;
using Xunit;

namespace Upstart.Weather.Service.UnitTests.Geocoding.Handlers
{
    public class GeocodingQueryHandlersTests
    {
        private readonly Mock<IGeocodingService> geocodingServiceMock;
        private readonly GeocodingQueryHandler handler;

        public GeocodingQueryHandlersTests()
        {
            geocodingServiceMock = new Mock<IGeocodingService>(MockBehavior.Strict);
            handler = new GeocodingQueryHandler(geocodingServiceMock.Object);
        }

        [Fact]
        public async Task GeocodingQueryHandler_ShouldTestTheReturnOkFromGeocodingApi()
        {
            var address = "1631 Pleasant Plains Rd Matthews, NC 28105";
            var query = new GetGeocodingDataByAddressQuery(address);
            var geocodingApiResponse = JsonConvert.DeserializeObject<GeocodingByAddressResult>(GeocodingFaker.JsonResponseOK);
            double expectedLat = 35.098015;
            double expectedLng = -80.72488;
            var expectedResponse = new GetGeocodingDataByAddressResponse(expectedLng, expectedLat);
            geocodingServiceMock.Setup(x => x.GetGeocodingDataFromAddressAsync(address, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(geocodingApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);
            var response = result.GetObjectValue<GetGeocodingDataByAddressResponse>();

            Assert.NotNull(response);
            Assert.True(result.IsSuccess);
            Assert.True(result.ResponseCode == HttpStatusCode.OK);
            Assert.Equal(expectedResponse.Lat, response.Lat);
            Assert.Equal(expectedResponse.Lng, response.Lng);
        }

        [Fact]
        public async Task GeocodingQueryHandler_ShouldTestTheReturnOkByNoContentFromGeocodingApi()
        {
            var address = "Some private address here";
            var query = new GetGeocodingDataByAddressQuery(address);
            var geocodingApiResponse = JsonConvert.DeserializeObject<GeocodingByAddressResult>(GeocodingFaker.JsonResponseNoContent);

            geocodingServiceMock.Setup(x => x.GetGeocodingDataFromAddressAsync(address, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(geocodingApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);
            var response = result.GetObjectValue<GetGeocodingDataByAddressResponse>();

            Assert.Null(response);
            Assert.True(result.IsSuccess);
            Assert.True(result.ResponseCode == HttpStatusCode.OK);
        }
    }
}
