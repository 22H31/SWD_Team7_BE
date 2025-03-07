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
        public class Results<T> : Result
        {
            public T? Value { get; set; } = default;

            protected Results(T value, HttpStatusCode statusCode) : base(statusCode)
            {
                Value = value;
            }

            protected Results(HttpStatusCode statuscode, List<ErrorsResult> errors) : base(statuscode, errors) { }

            public static Results<T> Success(T value, HttpStatusCode statusCode)
                => new Results<T>(value, statusCode);
            public static Results<T> Failure(HttpStatusCode statusCode, List<ErrorsResult> errors)
                => new Results<T>(statusCode, errors);

            public static Results<T> Ok(T value)
                => new Results<T>(value, HttpStatusCode.OK);

            public static Results<T> Created(T value)
                => new Results<T>(value, HttpStatusCode.Created);

            //public static implicit operator Result<T>(T value)
            //    => Success(value, HttpStatusCode.OK);

            public static new Results<T> NoContent()
                => new Results<T>(default, HttpStatusCode.NoContent);

            public static new Results<T> NotFound(List<ErrorsResult> errors)
                => new Results<T>(HttpStatusCode.NotFound, errors);

            public static new Results<T> BadRequest(List<ErrorsResult> errors)
                => new Results<T>(HttpStatusCode.BadRequest, errors);

            public static new Results<T> Unauthorized(List<ErrorsResult> errors)
                => new Results<T>(HttpStatusCode.Unauthorized, errors);

            public static new Results<T> Forbidden(List<ErrorsResult> errors)
                => new Results<T>(HttpStatusCode.Forbidden, errors);

        }

    }
}
