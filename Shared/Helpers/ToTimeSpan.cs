using WealthFlow.Domain.Enums;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Shared.Helpers
{
    public static class ToTimeSpan
    {
        public static TimeSpan covertToTimeSpan(ExpirationTime type, TimeUnitConversion timeUnit)
        {
            int expirationTime = Convert.ToInt32(type);

            switch(timeUnit)
            {
                case TimeUnitConversion.ToMinutes:
                    return TimeSpan.FromMinutes(expirationTime);
                case TimeUnitConversion.ToHours:
                    return TimeSpan.FromHours(expirationTime);
                case TimeUnitConversion.ToSeconds:
                    return TimeSpan.FromSeconds(expirationTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(expirationTime), expirationTime, "Invalid expiration type.");
            }
        }
            
    }
}
