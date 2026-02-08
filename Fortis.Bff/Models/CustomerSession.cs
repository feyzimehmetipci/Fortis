namespace Fortis.Bff.Models;

public class CustomerSession
{
    public Guid SessionId { get; set; } // Client'ın bildiği tek şey bu
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string DownstreamToken { get; set; } // Gerçek JWT burada saklı (Sır gibi)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}