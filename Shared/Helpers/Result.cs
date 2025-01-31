using static WealthFlow.Shared.Helpers.Enums.Enum;
using System.Net;
namespace WealthFlow.Shared.Helpers
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public HttpStatusCode HttpStatusCode{get; }
        public T? Data { get; }

        private Result(bool isSuccesss, HttpStatusCode httpStatusCode, string? message = null, T? data = default)
        {
            IsSuccess = isSuccesss;
            HttpStatusCode = httpStatusCode;
            Message = message;   
            Data = data;
        }

        public static Result<T> Success( HttpStatusCode httpStatusCode = HttpStatusCode.OK, string? successMessage = null)
            => new Result<T>(true, httpStatusCode, successMessage);
        public static Result<T> Success(string? successMessage = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
            =>new Result<T>(true, httpStatusCode, successMessage);

        public static Result<T> Success(T data, HttpStatusCode httpStatusCode = HttpStatusCode.OK, string? successMessage = null)
            =>new Result<T>(true, httpStatusCode, successMessage, data);

        public static Result<T> Failure(string errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
           => new Result<T>(false, httpStatusCode, errorMessage, default);

    }
}
