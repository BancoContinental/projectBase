using Continental.API.Core.Interfaces;
using Continental.API.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.Core;

public static class Install
{
    public static IServiceCollection AgregarCore(this IServiceCollection services)
    {
        services.AddTransient<ICuentaCorrienteService, CuentaCorrienteService>();

        return services;
    }
}