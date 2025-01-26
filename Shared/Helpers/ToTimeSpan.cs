using WealthFlow.Domain.Enums;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Shared.Helpers
{
    public static class ToTimeSpan
    {
        public static TimeSpan covertToTimeSpan(ExpirationTime time, TimeUnitConversion timeUnit)
        {
            int expirationTime = Convert.ToInt32(time);

            switch(timeUnit)
            {
                case TimeUnitConversion.DAYS:
                    return TimeSpan.FromDays(expirationTime);
                case TimeUnitConversion.MINUTES:
                    return TimeSpan.FromMinutes(expirationTime);
                case TimeUnitConversion.HOURS:
                    return TimeSpan.FromHours(expirationTime);
                case TimeUnitConversion.SECONDS:
                    return TimeSpan.FromSeconds(expirationTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(expirationTime), expirationTime, "Invalid expiration type.");
            }
        }
            
    }
}
