using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace FiapWebAluno.Tests;

public class ControllersSmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ControllersSmokeTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // ===========================
    // Endpoints simples (sem auth)
    // ===========================
    [Theory]
    [InlineData("/api/especie")]
    [InlineData("/api/canteiros")]
    [InlineData("/api/sensor")]
    public async Task Get_EndpointsSimples_Retornam200(string url)
    {
        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status 200-299
    }

    // ===========================
    // Endpoint avançado 1 – resumo horta (Gestor/Admin)
    // ===========================
    [Fact]
    public async Task Get_ResumoHorta_ComTokenGestor_Retorna200()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/relatorios/resumo-horta");
        request.Headers.Add("X-API-KEY", "gestor-token");

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    // ===========================
    // Endpoint avançado 2 – plano irrigação (Admin)
    // ===========================
    [Fact]
    public async Task Post_PlanoIrrigacao_ComTokenAdmin_Retorna200()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/relatorios/plano-irrigacao-sugerido");
        request.Headers.Add("X-API-KEY", "admin-token");

        request.Content = new StringContent("""
        {
          "areaM2": 100,
          "consumoLitrosPorM2Dia": 2,
          "diasPlanejados": 7
        }
        """);

        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }
}
