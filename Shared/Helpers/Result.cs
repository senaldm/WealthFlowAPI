namespace WealthFlow.Shared.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public int StatusCode{get; }

        private Result(bool isSuccesss, int statusCode, string? message = null)
        {
            IsSuccess = isSuccesss;
            StatusCode = statusCode;
            Message = message;   
        }

        public static Result Success(int StatusCode = 200)
            =>new Result(true, StatusCode, null);
        public static Result Success(string? successMessage = null, int StatusCode = 200)
            =>new Result(true, StatusCode, successMessage);
        public static Result Failure(string errorMessage, int StatusCode = 400)
            =>new Result(false, StatusCode, errorMessage);

    }
}
