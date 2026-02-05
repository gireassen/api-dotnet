# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Directory.Build.props ./
COPY Api/Api.csproj Api/
RUN dotnet restore Api/Api.csproj
COPY Api/ Api/
RUN dotnet publish Api/Api.csproj -c Release -o /out --no-restore

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS runtime
WORKDIR /app

# sec-update
RUN apt-get update \
 && apt-get install -y --no-install-recommends --only-upgrade openssl libssl3 \
 && apt-get purge -y --auto-remove gpgv gnupg dirmngr \
 && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /out/ ./
ENTRYPOINT ["dotnet","Api.dll"]
