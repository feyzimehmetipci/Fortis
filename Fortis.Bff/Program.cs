using Scalar.AspNetCore; // <-- 1. Bunu en tepeye ekle

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Bu kalacak, JSON'u bu üretiyor.

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