using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Dependencies;

public class BancoContinentalApiKeyProvider : IApiKeyProvider
{
    private readonly ILogger<BancoContinentalApiKeyProvider> _logger;
    public string ApiKey => _apiKey;
    private readonly string? _apiKey;

    public BancoContinentalApiKeyProvider(ILogger<BancoContinentalApiKeyProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _apiKey = configuration.GetValue<string>("ApiKey");

        if (_apiKey is null)
        {
            throw new ArgumentException("No se encuentra el ApiKey", nameof(ApiKey));
        }
    }

    public async Task<IApiKey> ProvideAsync(string key)
    {
        try
        {
            if (key.Equals(_apiKey))
            {
                _logger.LogDebug("Key valido");
                return new BancoContinentalApiKey(key);
            }

            _logger.LogWarning("Key no valido");

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

public class BancoContinentalApiKey : IApiKey
{
    public string Key { get; }
    public string OwnerName { get; } = "Banco Continental";
    public IReadOnlyCollection<Claim> Claims { get; }

    public BancoContinentalApiKey(string key)
    {
        Key = key;
    }
}
