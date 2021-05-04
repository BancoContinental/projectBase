using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Continental.API.WebApi.Dependencies
{
    public static class KeycloakDependencyInjection
    {
        private const string ConfigSection = "Keycloak:Jwt";

        public static IServiceCollection AgregarKeycloak(
            this IServiceCollection services, string configSection = null)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var tokenOptions  = new TokenOptions();
            configuration.GetSection(configSection ?? ConfigSection).Bind(tokenOptions);

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = tokenOptions.RequireHttps;
                options.Authority           = tokenOptions.Authority;
                options.Audience            = tokenOptions.Audience;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.SigninKey))
                };
            });

            return services;
        }
    }

    public class TokenOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SigninKey { get; set; }

        public string Authority { get; set; }

        public bool RequireHttps { get; set; }
    }
}