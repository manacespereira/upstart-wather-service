using System;
using System.Collections.Generic;
using System.Linq;
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
            var numberOfDays = 7;
            var query = new GetWeatherByAddressQuery(address, numberOfDays);
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


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public async Task WeatherQueryHandler_ShouldTestTheHappyScenarioUsingDinamicallyNumberOfDays(int numberOfDays)
        {
            var address = "1631 Pleasant Plains Rd Matthews, NC 28105";
            var numberOfDaysTimesTwo = numberOfDays * 2;
            var query = new GetWeatherByAddressQuery(address, numberOfDays);
            double expectedLat = 35.098015;
            double expectedLng = -80.72488;
            var gridsResponse = new GridsResult("valididurl", new WeatherProperties { GridX = 1, GridY = 2 });
            var weatherResponse = new WeatherResult
            {
                Id = "myweathermocked",
                Properties = new WeatherProperties
                {
                    Periods = new List<WeatherPeriod> {
                        new WeatherPeriod { Name = "MyPeriod 1" },
                        new WeatherPeriod { Name = "MyPeriod 2" },
                        new WeatherPeriod { Name = "MyPeriod 3" },
                        new WeatherPeriod { Name = "MyPeriod 4" },
                        new WeatherPeriod { Name = "MyPeriod 5" },
                        new WeatherPeriod { Name = "MyPeriod 6" },
                        new WeatherPeriod { Name = "MyPeriod 7" },
                        new WeatherPeriod { Name = "MyPeriod 8" },
                        new WeatherPeriod { Name = "MyPeriod 9" },
                        new WeatherPeriod { Name = "MyPeriod 10" },
                        new WeatherPeriod { Name = "MyPeriod 11" },
                        new WeatherPeriod { Name = "MyPeriod 12" },
                        new WeatherPeriod { Name = "MyPeriod 13" },
                        new WeatherPeriod { Name = "MyPeriod 14" },
                    },
                    Elevation = new ElevationArea()
                }
            };
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
            Assert.True(response.Periods.Count() == numberOfDaysTimesTwo);

            mediatorMock.Verify(x => x.Send(It.IsAny<GetGeocodingDataByAddressQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            weatherServiceMock.Verify(x => x.GetGridsByLatLngAsync(expectedLat, expectedLng), Times.Once);
            weatherServiceMock.Verify(x => x.GetWeatherByGridsAsync(gridsResponse.Properties.GridId, gridsResponse.Properties.GridX, gridsResponse.Properties.GridY), Times.Once);
        }

        [Fact]
        public async Task WeatherQueryHandler_ShouldTestTheScenarioWhenGeocodingQueryReturnsNoContent()
        {
            var address = "1631 Pleasant Plains Rd Matthews, NC 28105";
            var numberOfDays = 7;
            var query = new GetWeatherByAddressQuery(address, numberOfDays);

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
