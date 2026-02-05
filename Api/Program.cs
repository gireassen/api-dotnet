using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Чуть аккуратнее JSON по умолчанию
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    o.SerializerOptions.WriteIndented = false;
});

var app = builder.Build();

// Единый контракт
app.MapGet("/healthz", () => Results.Ok("ok"));
app.MapGet("/readyz", () => Results.Ok("ready"));

static IResult WhoAmI()
{
    var version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "";
    return Results.Ok(new
    {
        language = ".net",
        version = "dev",
        time = DateTimeOffset.UtcNow.ToString("O"),
    });
}

// Основной endpoint (ожидается при ingress rewrite /api/dotnet -> /)
app.MapGet("/api/whoami", () => WhoAmI());

// Alias (если ingress НЕ делает rewrite и прокидывает полный путь)
app.MapGet("/api/dotnet/whoami", () => WhoAmI());

// Сервис слушает на 8080 (важно для единообразия)
app.Run("http://0.0.0.0:8080");

// Нужно для WebApplicationFactory в тестах
public partial class Program { }
