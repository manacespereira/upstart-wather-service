using System.Linq;
using Upstart.Weather.Service.Domain.Geocoding.Queries;
using Xunit;

namespace Upstart.Weather.Service.UnitTests.Geocoding.Queries
{
    public class GetGeocodingDataByAddressQueryTests
    {
        [Theory]
        [InlineData("", "Address must be required")]
        [InlineData("add", "Address must be complete")]
        public void GetGeocodingDataByAddressQuery_ShouldTestInvalidQuery(string wrongOrNullAddress, string errorMessage)
        {
            var query = new GetGeocodingDataByAddressQuery(wrongOrNullAddress);

            query.Validate();

            Assert.True(query.Invalid);
            Assert.NotEmpty(query.Notifications);
            Assert.Contains(errorMessage, query.Notifications.Select(x => x.Message));
        }

        [Theory]
        [InlineData("3000, complete, address")]
        public void GetGeocodingDataByAddressQuery_ShouldTestValidQuery(string rightAddress)
        {
            var query = new GetGeocodingDataByAddressQuery(rightAddress);

            query.Validate();

            Assert.True(query.Valid);
            Assert.Empty(query.Notifications);
        }
    }
}