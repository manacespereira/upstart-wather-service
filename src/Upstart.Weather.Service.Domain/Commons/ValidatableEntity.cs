using Flunt.Notifications;
using Flunt.Validations;

namespace Upstart.Weather.Service.Domain.Commons
{
    public abstract class ValidatableEntity : Notifiable, IValidatable
    {
        public abstract void Validate();
    }
}
