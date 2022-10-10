using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ApiBanking.Tests;

public class EndpointsTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public EndpointsTests(ApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Api_Tiene_Swagger()
    {
        var response = await _client.GetAsync("/swagger");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Api_CuandoNoTienePath_RedirigeASwagger()
    {
        var response = await _client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Swagger");
    }

    [Fact]
    public async Task Api_Tiene_ReadinessEndpoint()
    {
        var response = await _client.GetAsync("/readiness");

        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Api_Tiene_LivenessEndpoint()
    {
        var response = await _client.GetAsync("/liveness");

        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }
}
