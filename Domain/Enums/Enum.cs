namespace WealthFlow.Domain.Enums
{
    public class Enum
    {
        public enum ExpirationTime
        {
            PASSWORD_VERIFICATION_TIME = 10,
            JWT_TOKEN_VERIFICATION_TIME = 7,
            EMAIL_VERIFICATION_TIME = 10
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

    }
}
