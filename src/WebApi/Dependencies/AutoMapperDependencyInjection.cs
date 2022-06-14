using System;
using AutoMapper;
using Continental.API.WebApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.WebApi.Dependencies;

public static class AutoMapperDependencyInjection
{
    public static IServiceCollection AgregarAutoMapper(this IServiceCollection services)
    {
        // Auto Mapper Configurations
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        var mapper = mappingConfig.CreateMapper();

        return services.AddSingleton(mapper);
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Entities.DiaHabil, DiaHabil>().ReverseMap();
        CreateMap<DateTime, string>().ConvertUsing(s => s.Date.ToString("dd/MM/yyyy"));
    }
}