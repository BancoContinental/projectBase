using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Continental.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarInfraestructura(this IServiceCollection services)
        {
            // Descomentar para usar Entity Framework Core
            // var config           = services.BuildServiceProvider().GetService<IConfiguration>();
            // var connectionString = config.GetConnectionString("ApiConsulta");
            // services.AddDbContext<OracleOracleDbContext>(o =>
            //     o.UseOracle(connectionString));

            // Cambiar por EfFechasRepository para usar Entity Framework Core
            services.AddTransient<IFechasRepository, DapperFechasRepository>();

            return services;
        }
    }
}
