using System;
using Flunt.Validations;
using Upstart.Weather.Service.Domain.Commons;

namespace Upstart.Weather.Service.Domain.Weather.Queries
{
    public class GetWeatherByAddressQuery : Query
    {
        public GetWeatherByAddressQuery(string address, int numberOfDays)
        {
            Address = address;
            NumberOfDays = numberOfDays;
        }

        public string Address { get; set; }
        public int NumberOfDays { get; set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .IsNotNullOrWhiteSpace(Address, nameof(Address), "Address is required")
                .HasMinLen(Address, 5, nameof(Address), "Address must be complete")
                .IsGreaterThan(NumberOfDays, 0, nameof(NumberOfDays), "You should select 1 day or more"));
        }
    }
}
