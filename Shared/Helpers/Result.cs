using static WealthFlow.Shared.Helpers.Enums.Enum;
using System.Net;
namespace WealthFlow.Shared.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public HttpStatusCode HttpStatusCode{get; }

        private Result(bool isSuccesss, HttpStatusCode httpStatusCode, string? message = null)
        {
            IsSuccess = isSuccesss;
            HttpStatusCode = httpStatusCode;
            Message = message;   
        }

        public static Result Success(HttpStatusCode httpStatusCode = HttpStatusCode.OK)
            =>new Result(true, httpStatusCode, null);
        public static Result Success(string? successMessage = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
            =>new Result(true, httpStatusCode, successMessage);
        public static Result Failure(string errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
            =>new Result(false, httpStatusCode, errorMessage);

    }
}
