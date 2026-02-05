using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

public class SmokeTests
{
    [Fact]
    public async Task Healthz_Returns200()
    {
        await using var app = new WebApplicationFactory<Program>();
        var client = app.CreateClient();

        var res = await client.GetAsync("/healthz");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Readyz_Returns200()
    {
        await using var app = new WebApplicationFactory<Program>();
        var client = app.CreateClient();

        var res = await client.GetAsync("/readyz");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Whoami_ReturnsJsonWithLanguage()
    {
        await using var app = new WebApplicationFactory<Program>();
        var client = app.CreateClient();

        var res = await client.GetAsync("/api/whoami");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);

        var json = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("language", out var lang));
        Assert.Equal(".net", lang.GetString());
    }
}
