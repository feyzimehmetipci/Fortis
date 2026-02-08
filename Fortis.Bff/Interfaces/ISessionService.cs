using Fortis.Bff.Models;

namespace Fortis.Bff.Interfaces;

public interface ISessionService
{
    Task<Guid> CreateSessionAsync(CustomerSession session);
    Task<CustomerSession?> GetSessionAsync(Guid sessionId);
    Task RemoveSessionAsync(Guid sessionId);
}