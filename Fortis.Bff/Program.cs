using Scalar.AspNetCore; 
using StackExchange.Redis;   
using Fortis.Bff.Interfaces; 
using Fortis.Bff.Services;   

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Bu kalacak, JSON'u bu üretiyor.

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
    ConnectionMultiplexer.Connect("127.0.0.1:6379,abortConnect=false"));

    // 2. Session Servisini Kaydet
builder.Services.AddScoped<ISessionService, RedisSessionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllDev", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Bu /openapi/v1.json adresini açar.
    app.MapScalarApiReference(); // <-- 2. İŞTE SİHİRLİ SATIR BU!
}

app.UseHttpsRedirection();

app.UseCors("AllowAllDev");

app.UseAuthorization();
app.MapControllers();

app.Run();