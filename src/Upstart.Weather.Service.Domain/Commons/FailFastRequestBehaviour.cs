using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Upstart.Weather.Service.Domain.Commons
{
    public class FailFastRequestBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ValidatableEntity, IRequest<TResponse> where TResponse : class, IResult
    {

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            request.Validate();

            return request.Invalid
                ? Result.BadRequest(request.Notifications) as TResponse
                : await next();
        }
    }
}
