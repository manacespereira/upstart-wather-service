using MediatR;

namespace Upstart.Weather.Service.Domain.Commons
{
    public abstract class Command : ValidatableEntity, IRequest<IResult> { }
}
