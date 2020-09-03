using Continental.API.Core.Interfaces;
using Continental.API.Infrastructure.Data;
using Continental.API.Infrastructure.DatabaseHelpers;
using Continental.API.Infrastructure.Repositories;
using Continental.API.Infrastructure.Settings;
using Continental.API.Infrastructure.Settings.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Continental.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarInfraestructura(this IServiceCollection services)
        {
            // Descomentar para usar Entity Framework Core
            // var config = services.BuildServiceProvider().GetService<IOptions<Configuraciones>>();
            // var conexiones = new ConexionBD(config.Value.SeteosBD);
            // var connectionString = conexiones.GetCadenaDeConexion(
            //     TiposCredenciales.SERVICIOS_CONSULTA,
            //     TiposDataSource.DATOSITA);
            // services.AddDbContext<OracleOracleDbContext>(o =>
            //     o.UseOracle(connectionString));

            // Cambiar por EfFechasRepository para usar Entity Framework Core
            services.AddTransient<IFechasRepository, DapperFechasRepository>();

            return services;
        }
    }
}
