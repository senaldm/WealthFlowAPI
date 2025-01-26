namespace WealthFlow.Domain.Enums
{
    public class Enum
    {
        public enum ExpirationTime
        {
            PasswordVerficationTime = 5
        }
        public enum  TimeUnitConversion
        {
            ToHours,
            ToMinutes,
            ToSeconds,
        }
        public enum VerificationType
        {
            PasswordVerification,
            EmailVerification
        }

    }
}
