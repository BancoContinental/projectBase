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
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddDbContext<OracleDbContext>(o =>
                o.UseOracle(config.GetConnectionString("Oracle")));

            services.AddTransient<IFechasRepository, FechasRepository>();

            return services;
        }
    }
}
