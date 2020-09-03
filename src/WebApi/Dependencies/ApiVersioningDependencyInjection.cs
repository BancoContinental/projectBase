using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.WebApi.Dependencies
{
    public static class ApiVersioningDependencyInjection
    {
        public static IServiceCollection AgregarVersionamientoApi(
            this IServiceCollection services,
            int majorVersion,
            int minorVersion)
        {
            return services.AddApiVersioning(c =>
            {
                c.DefaultApiVersion                   = new ApiVersion(majorVersion, minorVersion);
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.ReportApiVersions                   = true;
            });
        }
    }
}
