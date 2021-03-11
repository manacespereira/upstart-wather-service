using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upstart.Weather.Service.Api.Commons;
using Upstart.Weather.Service.Domain.Weather.Queries;

namespace Upstart.Weather.Service.Api.Controllers
{
    [Route("api/v1/weather")]
    [ApiController]
    public class WeatherController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get(string address)
        {
            var response = await Mediator.Send(new GetWeatherByAddressQuery(address));
            return AsResult(response);
        }
    }
}