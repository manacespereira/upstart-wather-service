using System;
using System.Linq;
using Upstart.Weather.Service.Domain.Weather.Queries;
using Xunit;

namespace Upstart.Weather.Service.UnitTests.Weather.Queries
{
    public class GetWeatherByAddressQueryTests
    {


        [Theory]
        [InlineData("", 1, "Address must be required")]
        [InlineData("add", 1, "Address must be complete")]
        [InlineData("address complete but 0 days to get the data", 0, "You should select 1 day or more")]
        public void GetWeatherByLatLngQuery_ShouldTestInvalidQuery(string wrongOrNullAddress, int numberOfDays, string errorMessage)
        {
            var query = new GetWeatherByAddressQuery(wrongOrNullAddress, numberOfDays);

            query.Validate();

            Assert.True(query.Invalid);
            Assert.NotEmpty(query.Notifications);
            Assert.Contains(errorMessage, query.Notifications.Select(x => x.Message));
        }

        [Theory]
        [InlineData("3000, complete, address", 7)]
        public void GetWeatherByLatLngQuery_ShouldTestValidQuery(string rightAddress, int numberOfDays)
        {
            var query = new GetWeatherByAddressQuery(rightAddress, numberOfDays);

            query.Validate();

            Assert.True(query.Valid);
            Assert.Empty(query.Notifications);
        }
    }
}
