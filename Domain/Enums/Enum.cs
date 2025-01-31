namespace WealthFlow.Domain.Enums
{
    public class Enum
    {
        public enum ExpirationTime
        {
            PASSWORD_VERIFICATION = 10,
            JWT_TOKEN_VERIFICATION = 7,
            EMAIL_VERIFICATION = 10,
            REFRESH_TOKEN = 7,
            JWT_COOKIE = 20,
               
        }
        public enum  TimeUnitConversion
        {
            HOURS,
            MINUTES,
            SECONDS,
            DAYS
        }
        public enum VerificationType
        {
            PASSWORD_VERIFICATION,
            EMAIL_VERIFICATION,
            JWT_TOKEN_VERIFICATION


        }

        public enum  TransactionType
        {
            INCOME,
            EXPENCES

        }
    }
}
