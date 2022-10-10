using System;
using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Clients;
using Continental.API.Infrastructure.Database;
using Continental.API.Infrastructure.Database.Helpers;
using Continental.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.Infrastructure;

public static class Install
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        var config = services.BuildServiceProvider().GetService<IConfiguration>();

        services.AddDbContext<OracleDbContext>(o =>
            o.UseOracle(config.GetConnectionString("Oracle")));
        services.AddDbContext<ActiveDbContext>(o =>
        {
            o.UseOracle(config.GetConnectionString("Active"));
            o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddScoped<Func<TipoConexion, IApplicationDbContext>>(serviceProvider => key =>
        {
            return key switch
            {
                TipoConexion.Active => serviceProvider.GetRequiredService<ActiveDbContext>(),
                TipoConexion.Oracle => serviceProvider.GetRequiredService<OracleDbContext>(),
                _ => throw new ApplicationException("DbContext no encontrado")
            };
        });

        services.AddScoped<IAppDb, AppAppDb>();

        services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();

        services.AddScoped<ICuentaCorrienteRepository, CuentaCorrienteDbRepository>();

        services.AddHttpClient("ApiSaldo", options =>
        {
            options.BaseAddress = new Uri(config.GetConnectionString("ApiSaldo"));
        });

        services.AddTransient<ISaldoService, SaldoApiService>();

        return services;
    }
}