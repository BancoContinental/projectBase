using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Continental.API.WebApi.Dependencies;

public static class SwaggerDependencyInjection
{
    public static IServiceCollection AgregarDocumentacionSwagger(this IServiceCollection services, string nombreApi)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "Continental API",
                Version     = "v1",
                Description = $"Documentacion para el uso de API de {nombreApi}",
                Contact = new OpenApiContact
                {
                    Email = "informatica@bancontinental.com.py",
                    Name  = "Departamento de Tecnologia"
                }
            });
            var xmlFile = Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml");
            c.IncludeXmlComments(xmlFile);
        });
    }
}