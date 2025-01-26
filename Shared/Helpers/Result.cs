using static WealthFlow.Shared.Helpers.Enums.Enum;

namespace WealthFlow.Shared.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public int HttpStatusCode{get; }

        private Result(bool isSuccesss, int httpStatusCode, string? message = null)
        {
            IsSuccess = isSuccesss;
            HttpStatusCode = httpStatusCode;
            Message = message;   
        }

        public static Result Success(int httpStatusCode = (int)StatusCode.OK)
            =>new Result(true, httpStatusCode, null);
        public static Result Success(string? successMessage = null, int httpStatusCode = (int)StatusCode.OK)
            =>new Result(true, httpStatusCode, successMessage);
        public static Result Failure(string errorMessage, int httpStatusCode = (int)StatusCode.BAD_REQUEST)
            =>new Result(false, httpStatusCode, errorMessage);

    }
}
