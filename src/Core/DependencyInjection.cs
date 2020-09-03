using Continental.API.Core.Interfaces;
using Continental.API.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarCore(this IServiceCollection services)
        {
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<IFechasService, FechasService>();

            return services;
        }
    }
}
