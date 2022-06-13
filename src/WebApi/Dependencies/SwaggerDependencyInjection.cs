using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Continental.API.WebApi.Dependencies
{
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
                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            });
        }
    }

    public class RemoveVersionParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }

    public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }

            swaggerDoc.Paths = paths;
        }
    }
}
