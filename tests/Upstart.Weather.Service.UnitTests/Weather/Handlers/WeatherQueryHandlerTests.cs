using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using Upstart.Weather.Service.Application.QueryHandlers;
using Upstart.Weather.Service.Application.Responses;
using Upstart.Weather.Service.Domain.Commons;
using Upstart.Weather.Service.Domain.Geocoding.Models;
using Upstart.Weather.Service.Domain.Geocoding.Queries;
using Upstart.Weather.Service.Domain.Weather.Models;
using Upstart.Weather.Service.Domain.Weather.Queries;
using Upstart.Weather.Service.Infra.ExternalServices;
using Xunit;

namespace Upstart.Weather.Service.UnitTests.Weather.Handlers
{
    public class WeatherQueryHandlerTests
    {
        private readonly Mock<IWeatherService> weatherServiceMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly WeatherQueryHandler handler;

        public WeatherQueryHandlerTests()
        {
            weatherServiceMock = new Mock<IWeatherService>(MockBehavior.Strict);
            mediatorMock = new Mock<IMediator>(MockBehavior.Strict);

            handler = new WeatherQueryHandler(weatherServiceMock.Object, mediatorMock.Object, new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public async Task WeatherQueryHandler_ShouldTestTheHappyScenario()
        {
            var address = "1631 Pleasant Plains Rd Matthews, NC 28105";
            var query = new GetWeatherByAddressQuery(address);
            double expectedLat = 35.098015;
            double expectedLng = -80.72488;
            var gridsResponse = new GridsResult("valididurl", new WeatherProperties { GridX = 1, GridY = 2 });
            var weatherResponse = new WeatherResult { Id = "myweathermocked", Properties = new WeatherProperties { Periods = new List<WeatherPeriod> { new WeatherPeriod { Name = "MyPeriod" } }, Elevation = new ElevationArea() } };
            var mediatorGeocodingResponse = new GetGeocodingDataByAddressResponse(expectedLng, expectedLat);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetGeocodingDataByAddressQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(mediatorGeocodingResponse));
            weatherServiceMock.Setup(x => x.GetGridsByLatLngAsync(expectedLat, expectedLng)).ReturnsAsync(gridsResponse);
            weatherServiceMock.Setup(x => x.GetWeatherByGridsAsync(gridsResponse.Properties.GridId, gridsResponse.Properties.GridX, gridsResponse.Properties.GridY)).ReturnsAsync(weatherResponse);

            var result = await handler.Handle(query, CancellationToken.None);
            var response = result.GetObjectValue<GetWeatherByAddressResponse>();

            Assert.NotNull(response);
            Assert.True(result.IsSuccess);
            Assert.NotNull(response.Elevation);
            Assert.NotNull(response.Periods);
            Assert.NotEmpty(response.Periods);

            mediatorMock.Verify(x => x.Send(It.IsAny<GetGeocodingDataByAddressQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            weatherServiceMock.Verify(x => x.GetGridsByLatLngAsync(expectedLat, expectedLng), Times.Once);
            weatherServiceMock.Verify(x => x.GetWeatherByGridsAsync(gridsResponse.Properties.GridId, gridsResponse.Properties.GridX, gridsResponse.Properties.GridY), Times.Once);
        }

        [Fact]
        public async Task WeatherQueryHandler_ShouldTestTheScenarioWhenGeocodingQueryReturnsNoContent()
        {
            var address = "1631 Pleasant Plains Rd Matthews, NC 28105";
            var query = new GetWeatherByAddressQuery(address);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetGeocodingDataByAddressQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());

            var result = await handler.Handle(query, CancellationToken.None);
            var response = result.GetObjectValue<GetWeatherByAddressResponse>();

            Assert.Null(response);
            Assert.True(result.IsFailure);
            mediatorMock.Verify(x => x.Send(It.IsAny<GetGeocodingDataByAddressQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            weatherServiceMock.Verify(x => x.GetGridsByLatLngAsync(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
            weatherServiceMock.Verify(x => x.GetWeatherByGridsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
