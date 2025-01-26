namespace WealthFlow.Shared.Helpers.Enums
{
    public class Enum
    {
        public enum StatusCode
        {
            OK = 200,
            CREATED = 201,
            OK_WITH_NO_CONTENT = 204,

            BAD_REQUEST = 400,
            UNAUTHORIZED = 401,
            FORBIDDEN = 402,
            NOT_FOUND = 404,
            METHODS_NOT_ALLOWED = 405,
            CONFLICT = 409,

            INTERNAL_SERVER_ERROR = 500,
            BAD_GATEWAY = 502,
            SERVICE_UNAVAILABLE = 503,
        }
    }
}
