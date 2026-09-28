namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Caching;

public interface ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    public Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken);
    public Task RemoveAsync(string key, CancellationToken cancellationToken);
}
