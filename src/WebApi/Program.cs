using System;
using AspNetCore.Authentication.ApiKey;
using Continental.API.Core;
using Continental.API.Infrastructure;
using Continental.API.WebApi.Dependencies;
using Continental.API.WebApi.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder();

    // Serilog
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
    Log.Information("Iniciando {ApplicationName}", builder.Configuration["Serilog:Properties:ApplicationName"]);

    builder.Services.AddHeaderPropagation()
        .AddHttpContextAccessor()
        .AddHealthChecks(builder.Configuration)
        .AgregarCore()
        .AgregarInfraestructura(builder.Configuration)
        .AgregarDocumentacionSwagger(builder.Configuration["Serilog:Properties:ApplicationName"])
        .AgregarAutoMapper()
        .AgregarFluentValidation()
        .AddControllers();

    builder.Services.AddFeatureManagement();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
        .AddApiKeyInHeader<BancoContinentalApiKeyProvider>(options =>
        {
            options.Realm   = builder.Configuration["ApiKeyConfiguration:Realm"];
            options.KeyName = builder.Configuration["ApiKeyConfiguration:Header"];
        });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            builder.Configuration["Serilog:Properties:ApplicationName"]);
    });

    app.UseSerilogRequestLogging(opts
        => opts.EnrichDiagnosticContext = LogRequestEnricher.EnrichFromRequest);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/readiness", new HealthCheckOptions
    {
        Predicate      = _ => true,
        ResponseWriter = Extension.HealthChecksResponseWriter
    });
    app.MapHealthChecks("/liveness", new HealthCheckOptions
    {
        Predicate      = r => r.Name.Contains("self"),
    });

    app.MapGet("/", ()
        => Results.Redirect("swagger", true))
        .ExcludeFromDescription();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La API fallo al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
