# api-dotnet

Minimal API на .NET (по умолчанию net8.0) с контрактом:

- GET /healthz -> 200 "ok"
- GET /readyz  -> 200 "ready"
- GET /api/whoami -> JSON { language, version, time }

Дополнительно:
- GET /api/dotnet/whoami (alias на случай если Ingress не делает rewrite префикса)

## Локальный запуск
```bash
dotnet test Api.Tests/Api.Tests.csproj
dotnet run --project Api
```


# for commit