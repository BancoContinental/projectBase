using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.WebApi.Dependencies
{
    public static class ConfigurationDependencyInjection
    {
        public static IServiceCollection AgregarConfiguraciones(this IServiceCollection services, IConfiguration configuration)
        {
            // mapeo de configuracion a objeto -> capa Seguridad
            services.Configure<Seguridad.Settings.Configuraciones>(configuration.GetSection("Configuraciones"));

            // mapeo de configuracion a objeto -> capa Infraestructura
            services.Configure<Infrastructure.Settings.Configuraciones>(configuration.GetSection("Configuraciones"));

            return services;
        }
    }
}