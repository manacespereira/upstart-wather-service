using System.Threading.Tasks;
using Refit;
using Upstart.Weather.Service.Domain.Weather.Models;

namespace Upstart.Weather.Service.Infra.ExternalServices
{
    [Headers(
        "User-Agent: (myweatherapp.com, contact@myweatherapp.com)"
    )]
    public interface IWeatherService
    {
        [Get("/points/{lat},{lng}")]
        Task<GridsResult> GetGridsByLatLngAsync(double lat, double lng);

        [Get("/gridpoints/{gridId}/{gridX},{gridY}/forecast")]
        Task<WeatherResult> GetWeatherByGridsAsync(string gridId, int gridX, int gridY);
    }
}