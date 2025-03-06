using System.Net;
using System.Text.Json.Serialization;
using BE_Team7.Shared.ErrorModel;

namespace BE_Team7.Shared
{
    public class Result
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }
        protected Result(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public List<ErrorsResult>? Errors { get; private set; }

        protected Result(HttpStatusCode statusCode, List<ErrorsResult> errors)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
        public class Result<T> : Result
        {
            public T? Value { get; set; } = default;

            protected Result(T value, HttpStatusCode statusCode) : base(statusCode)
            {
                Value = value;
            }

            protected Result(HttpStatusCode statuscode, List<ErrorsResult> errors) : base(statuscode, errors) { }

            public static Result<T> Success(T value, HttpStatusCode statusCode)
                => new Result<T>(value, statusCode);
            public static Result<T> Failure(HttpStatusCode statusCode, List<ErrorsResult> errors)
                => new Result<T>(statusCode, errors);

            public static Result<T> Ok(T value)
                => new Result<T>(value, HttpStatusCode.OK);

            public static Result<T> Created(T value)
                => new Result<T>(value, HttpStatusCode.Created);

            //public static implicit operator Result<T>(T value)
            //    => Success(value, HttpStatusCode.OK);

            public static new Result<T> NoContent()
                => new Result<T>(default, HttpStatusCode.NoContent);

            public static new Result<T> NotFound(List<ErrorsResult> errors)
                => new Result<T>(HttpStatusCode.NotFound, errors);

            public static new Result<T> BadRequest(List<ErrorsResult> errors)
                => new Result<T>(HttpStatusCode.BadRequest, errors);

            public static new Result<T> Unauthorized(List<ErrorsResult> errors)
                => new Result<T>(HttpStatusCode.Unauthorized, errors);

            public static new Result<T> Forbidden(List<ErrorsResult> errors)
                => new Result<T>(HttpStatusCode.Forbidden, errors);

        }

    }
}
