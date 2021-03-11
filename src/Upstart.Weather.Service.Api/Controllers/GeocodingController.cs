using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upstart.Weather.Service.Api.Commons;
using Upstart.Weather.Service.Domain.Geocoding.Queries;

namespace Upstart.Weather.Service.Api.Controllers
{
    [Route("api/v1/geocoding")]
    [ApiController]
    public class GeocodingController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get(string address)
        {
            var response = await Mediator.Send(new GetGeocodingDataByAddressQuery(address));
            return AsResult(response);
        }
    }
}
