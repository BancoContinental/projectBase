using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Continental.API.WebApi.Dependencies;

public class BancoContinentalApiKeyProvider : IApiKeyProvider
{
    private readonly ILogger<BancoContinentalApiKeyProvider> _logger;
    public string ApiKey => _apiKey;
    private readonly string _apiKey;

    public BancoContinentalApiKeyProvider(ILogger<BancoContinentalApiKeyProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _apiKey = configuration.GetValue<string>("ApiKeyConfiguration:key");

        if (_apiKey is null)
        {
            throw new ArgumentException("No se encuentra el ApiKey", nameof(ApiKey));
        }
    }

    public Task<IApiKey> ProvideAsync(string key)
    {
        try
        {
            if (key.Equals(_apiKey))
            {
                _logger.LogDebug("Key valido");
                return Task.FromResult<IApiKey>(new BancoContinentalApiKey(key));
            }

            _logger.LogWarning("Key no valido");

            return Task.FromResult<IApiKey>(null);
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

public class ApiKeyFilter : IOperationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = _configuration.GetValue<string>("ApiKeyConfiguration:Header"),
            In   = ParameterLocation.Header,
            Schema = new OpenApiSchema
            {
                Type = "string"
            },
            AllowEmptyValue = false,
            Required        = true
        });
    }
}