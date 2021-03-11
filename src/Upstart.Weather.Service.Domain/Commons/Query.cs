using MediatR;

namespace Upstart.Weather.Service.Domain.Commons
{
    public abstract class Query : ValidatableEntity, IRequest<IResult> { }
}
