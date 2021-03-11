using System.Collections.Generic;
using System.Net;
using Flunt.Notifications;

namespace Upstart.Weather.Service.Domain.Commons
{
    public class Result : IResult
    {
        public Result(object value = null, bool isSuccess = true, IReadOnlyCollection<Notification> errors = null, HttpStatusCode responseCode = HttpStatusCode.OK)
        {
            Value = value;
            IsSuccess = isSuccess;
            Errors = errors;
            ResponseCode = responseCode;
        }

        public bool HasValue => Value != null;
        public object Value { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public HttpStatusCode ResponseCode { get; }
        public IReadOnlyCollection<Notification> Errors { get; }
        public T GetObjectValue<T>() => (T)Value;

        public static Result Ok(object value = null) => new Result(value: value);
        public static Result Accepted(object value = null) => new Result(value: value, responseCode: HttpStatusCode.Accepted);
        public static Result Fail(IReadOnlyCollection<Notification> errors = null) => new Result(errors: errors, isSuccess: false, responseCode: HttpStatusCode.InternalServerError);
        public static Result BadRequest(IReadOnlyCollection<Notification> errors = null) => new Result(errors: errors, isSuccess: false, responseCode: HttpStatusCode.BadRequest);
    }
}
