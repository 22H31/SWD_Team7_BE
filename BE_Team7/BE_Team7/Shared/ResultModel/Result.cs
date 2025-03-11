using BE_Team7.Shared.ErrorModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageManagementAPI.Shared.ResultModel
{
    public class Result
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }

        public List<ErrorsResult>? Errors { get; private set; }

        [JsonIgnore]
        public bool IsSuccess => Errors is null;

        public Result()
        {
        }

        protected Result(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        protected Result(HttpStatusCode statusCode, List<ErrorsResult> errors)
        {
            Errors = errors;
            StatusCode = statusCode;
        }

        public static Result Success(HttpStatusCode statusCode)
            => new Result(statusCode);

        public static Result NoContent()
            => new Result(HttpStatusCode.NoContent);

        public static Result Ok()
           => new Result(HttpStatusCode.OK);

        public static Result Failure(HttpStatusCode statusCode, List<ErrorsResult> errors)
           => new Result(statusCode, errors);

        public static Result NotFound(List<ErrorsResult> errors)
            => new Result(HttpStatusCode.NotFound, errors);

        public static Result BadRequest(List<ErrorsResult> errors)
             => new Result(HttpStatusCode.BadRequest, errors);

        public static Result Unauthorized(List<ErrorsResult> errors)
            => new Result(HttpStatusCode.Unauthorized, errors);

        public static Result Forbidden(List<ErrorsResult> errors)
            => new Result(HttpStatusCode.Forbidden, errors);

        //public static implicit operator Result(HttpStatusCode statusCode)
        //    => Success(statusCode);

        //public static implicit operator Result(List<ErrorsResult> errors)
        //    => Failure(HttpStatusCode.BadRequest, errors);

        public virtual TResult Map<TResult>(Func<Result, TResult> onSuccess, Func<Result, TResult> onFailure)
            => IsSuccess ? onSuccess(this) : onFailure(this);

        public virtual async Task<TResult> Map<TResult>(Func<Result, Task<TResult>> onSuccess, Func<Result, Task<TResult>> onFailure)
            => IsSuccess ? await onSuccess(this).ConfigureAwait(false) : await onFailure(this).ConfigureAwait(false);


        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(this, options);
        }
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
        public static new Result<T> Failure(HttpStatusCode statusCode, List<ErrorsResult> errors)
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