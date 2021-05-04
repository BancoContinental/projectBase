using Continental.API.Core;
using Continental.API.Infrastructure;
using Continental.API.WebApi.Dependencies;
using Continental.API.WebApi.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Continental.API.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AgregarConfiguraciones(Configuration)
                .AgregarCore()
                .AgregarInfraestructura()
                .AgregarDocumentacionSwagger(typeof(Startup).Assembly.FullName)
                .AgregarVersionamientoApi(1, 0)
                .AgregarAutoMapper()
                .AddControllers()
                .AgregarFluentValidation(services)
                .AgregarKeycloak();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSerilogRequestLogging(opts
                => opts.EnrichDiagnosticContext = LogRequestEnricher.EnrichFromRequest);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Continental API");
                });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
