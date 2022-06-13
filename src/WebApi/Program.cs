using System;
using Continental.API.Core;
using Continental.API.Infrastructure;
using Continental.API.WebApi.Dependencies;
using Continental.API.WebApi.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder();

    // Serilog
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
    Log.Information("Iniciando {ApplicationName}", builder.Configuration["Serilog:Properties:ApplicationName"]);

    builder.Services.AddHeaderPropagation()
        .AgregarConfiguraciones(builder.Configuration)
        .AgregarCore()
        .AgregarInfraestructura()
        .AgregarDocumentacionSwagger(builder.Configuration["Serilog:Properties:ApplicationName"])
        .AgregarVersionamientoApi(1, 0)
        .AgregarAutoMapper()
        .AddControllers()
        .AgregarFluentValidation(builder.Services);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(o => { o.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Configuration["Serilog:Properties:ApplicationName"]); });

    app.UseSerilogRequestLogging(opts
        => opts.EnrichDiagnosticContext = LogRequestEnricher.EnrichFromRequest);

    app.MapControllers();

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
