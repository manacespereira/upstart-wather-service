using System.Threading.Tasks;
using Refit;
using Upstart.Weather.Service.Domain.Geocoding.Models;

namespace Upstart.Weather.Service.Infra.ExternalServices
{
    public interface IGeocodingService
    {
        [Get("/geocoder/locations/onelineaddress")]
        Task<GeocodingByAddressResult> GetGeocodingDataFromAddressAsync([Query] string address, [Query] string benchmark = "2020", [Query] string format = "json");
    }
}