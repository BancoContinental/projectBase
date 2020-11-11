using System;
using System.Collections.Generic;
using System.Text;
using Continental.API.Core;
using Continental.API.Infrastructure;
using Continental.API.Seguridad;
using Continental.API.WebApi.Dependencies;
using Continental.API.WebApi.Logger;
using Continental.API.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Continental.API.WebApi
{
    public class Startup
    {
        private IOperacionesToken _operacionesToken;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // registro de clases para DI.
            services.AddTransient<IOperacionesToken, OperacionesToken>();

            services.AgregarConfiguraciones(Configuration)
                .AgregarCore()
                .AgregarInfraestructura()
                .AgregarDocumentacionSwagger(typeof(Startup).Assembly.FullName)
                .AgregarVersionamientoApi(1, 0)
                .AgregarAutoMapper()
                .AddControllers()
                .AgregarFluentValidation(services);

            AgregarAutenticacion(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOperacionesToken operacionesToken)
        {
            // Injectamos el servicio de seguridad, la necesitamos en esta clase.
            _operacionesToken = operacionesToken;

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

            app.UseVerifyTokenSenderMiddleWare();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private IServiceCollection AgregarAutenticacion(IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                // Specify what in the JWT needs to be checked
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,

                // Specify the valid issue from appsettings.json
                ValidIssuer = Configuration["Jwt:Issuer"],

                // Specify the tenant API keys as the valid audiences
                ValidAudience = Configuration["Jwt:Audience"],

                // Checks de time validation of Token
                LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                {
                    // Additional checks can be performed on the SecurityToken or the validationParameters.
                    return (expires.HasValue && DateTime.UtcNow < expires) ? true : false;
                },

                // Resolve witch SecretKey use to validate the token sign
                IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                {
                    // get the SecretKey from DB
                    var secretKey = _operacionesToken.GetKeyDelToken(kid);

                    List<SecurityKey> keys = new List<SecurityKey>();

                    if (!string.IsNullOrEmpty(secretKey))
                    {
                        keys.Add(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)));
                    }

                    return keys;
                },
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            return services;
        }
    }
}
