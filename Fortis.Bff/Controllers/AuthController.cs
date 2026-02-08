using Microsoft.AspNetCore.Mvc;
using Fortis.Bff.Interfaces;
using Fortis.Bff.Models;

namespace Fortis.Bff.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly ILogger<AuthController> _logger;

    // Dependency Injection ile Servisimizi alıyoruz
    public AuthController(ISessionService sessionService, ILogger<AuthController> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Mock Authentication (İleride burası Downstream Identity Servisine gidecek)
        if (request.Email == "admin@fortis.com" && request.Password == "123456")
        {
            // 2. Başarılı ise Session Oluştur
            var newSession = new CustomerSession
            {
                UserId = "101", // Mock User ID
                Email = request.Email,
                FullName = "Admin User",
                DownstreamToken = "mock-jwt-token-xyz-123", // İleride gerçek JWT olacak
                Roles = new List<string> { "Admin", "User" } // Rolleri de saklayalım
            };

            var sessionId = await _sessionService.CreateSessionAsync(newSession);

            _logger.LogInformation("Kullanıcı {Email} giriş yaptı. SessionID: {SessionId}", request.Email, sessionId);

            // 3. Client'a sadece SessionID dön
            return Ok(new { SessionId = sessionId });
        }

        _logger.LogWarning("Başarısız giriş denemesi: {Email}", request.Email);
        return Unauthorized(new { Message = "Email veya şifre hatalı!" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "X-Session-ID")] Guid sessionId)
    {
        await _sessionService.RemoveSessionAsync(sessionId);
        return Ok(new { Message = "Çıkış yapıldı." });
    }
}