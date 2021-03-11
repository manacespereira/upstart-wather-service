using System;
using System.Collections.Generic;
using System.Net;
using Flunt.Notifications;

namespace Upstart.Weather.Service.Domain.Commons
{
    public interface IResult
    {
        bool IsSuccess { get; }

        bool IsFailure { get; }

        bool HasValue { get; }

        HttpStatusCode ResponseCode { get; }

        IReadOnlyCollection<Notification> Errors { get; }

        T GetObjectValue<T>();
    }
}
