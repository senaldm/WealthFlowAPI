namespace WealthFlow.Application.Caching.Interfaces
{
    public interface ICacheService
    {
        Task<bool> StoreAsync(string key, string value, TimeSpan expiration);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
    }
}
