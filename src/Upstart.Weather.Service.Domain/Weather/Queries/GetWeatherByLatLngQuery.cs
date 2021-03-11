using System;
using Flunt.Validations;
using Upstart.Weather.Service.Domain.Commons;

namespace Upstart.Weather.Service.Domain.Weather.Queries
{
    public class GetWeatherByAddressQuery : Query
    {
        public GetWeatherByAddressQuery(string address)
        {
            Address = address;
        }

        public string Address { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .IsNotNullOrWhiteSpace(Address, nameof(Address), "Address must be required")
                .HasMinLen(Address, 5, nameof(Address), "Address must be complete"));
        }
    }
}
