using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Continental.API.Core.Contracts.Entities;
using Continental.API.Core.Interfaces;

namespace Continental.API.Infrastructure.Clients;

public class SaldoApiService : ISaldoService
{
    private readonly IHttpClientFactory _clientFactory;

    public SaldoApiService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<SaldoCuenta> ObtenerSaldo(string cuenta)
    {
        var client = _clientFactory.CreateClient("ApiSaldo");

        var response = await client.GetFromJsonAsync<SaldoCuenta>($"/ms-consulta-saldo/v1/api/cuenta/{cuenta}/saldo");

        return response;
    }
}