using System;
using System.Linq;
using Upstart.Weather.Service.Domain.Weather.Queries;
using Xunit;

namespace Upstart.Weather.Service.UnitTests.Weather.Queries
{
    public class GetWeatherByAddressQueryTests
    {


        [Theory]
        [InlineData("", "Address must be required")]
        [InlineData("add", "Address must be complete")]
        public void GetWeatherByLatLngQuery_ShouldTestInvalidQuery(string wrongOrNullAddress, string errorMessage)
        {
            var query = new GetWeatherByAddressQuery(wrongOrNullAddress);

            query.Validate();

            Assert.True(query.Invalid);
            Assert.NotEmpty(query.Notifications);
            Assert.Contains(errorMessage, query.Notifications.Select(x => x.Message));
        }

        [Theory]
        [InlineData("3000, complete, address")]
        public void GetWeatherByLatLngQuery_ShouldTestValidQuery(string rightAddress)
        {
            var query = new GetWeatherByAddressQuery(rightAddress);

            query.Validate();

            Assert.True(query.Valid);
            Assert.Empty(query.Notifications);
        }
    }
}
