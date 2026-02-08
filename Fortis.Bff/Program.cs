using Scalar.AspNetCore; 
using StackExchange.Redis;   
using Fortis.Bff.Interfaces; 
using Fortis.Bff.Services;   

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Bu kalacak, JSON'u bu üretiyor.

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
    ConnectionMultiplexer.Connect("localhost:6379"));

    // 2. Session Servisini Kaydet
builder.Services.AddScoped<ISessionService, RedisSessionService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Bu /openapi/v1.json adresini açar.
    app.MapScalarApiReference(); // <-- 2. İŞTE SİHİRLİ SATIR BU!
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();