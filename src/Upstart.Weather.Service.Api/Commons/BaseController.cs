using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Upstart.Weather.Service.Domain.Commons;

namespace Upstart.Weather.Service.Api.Commons
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult AsResult(IResult result)
        {
            return result.IsFailure
                ? StatusCode((int)result.ResponseCode, result.Errors)
                : StatusCode((int)result.ResponseCode, result.GetObjectValue<object>());
        }
    }
}
