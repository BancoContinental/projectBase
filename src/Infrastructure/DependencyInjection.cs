using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Continental.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetService<IConfiguration>();
        services.AddDbContext<IOracleCommandsDbContext, OracleCommandsDbContext>(o =>
            o.UseOracle(config.GetConnectionString("Oracle")));
        services.AddDbContext<IOracleQueriesDbContext, OracleQueriesDbContext>(o =>
            o.UseOracle(config.GetConnectionString("Active")));

        services.AddScoped<IAppDbContext, AppDbContext>();

        services.AddTransient<IFechasQueriesRepository, FechasQueriesRepository>();

        return services;
    }
}