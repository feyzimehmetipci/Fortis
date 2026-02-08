using System.Text.Json;
using Fortis.Bff.Interfaces;
using Fortis.Bff.Models;
using StackExchange.Redis;

namespace Fortis.Bff.Services;

public class RedisSessionService : ISessionService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(30); // 30 dk hareketsizlikte oturum düşer

    public RedisSessionService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = redis.GetDatabase();
    }

    public async Task<Guid> CreateSessionAsync(CustomerSession session)
    {
        session.SessionId = Guid.NewGuid();
        var key = $"session:{session.SessionId}";
        var value = JsonSerializer.Serialize(session);

        // Veriyi yaz ve expire süresi koy
        await _db.StringSetAsync(key, value, _sessionTimeout);

        return session.SessionId;
    }

    public async Task<CustomerSession?> GetSessionAsync(Guid sessionId)
    {
        var key = $"session:{sessionId}";
        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return null;

        // Her okumada süreyi uzatalım (Sliding Expiration)
        await _db.KeyExpireAsync(key, _sessionTimeout);

        return JsonSerializer.Deserialize<CustomerSession>(value);
    }

    public async Task RemoveSessionAsync(Guid sessionId)
    {
        var key = $"session:{sessionId}";
        await _db.KeyDeleteAsync(key);
    }
}