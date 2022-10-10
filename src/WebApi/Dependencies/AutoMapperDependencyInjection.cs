using AutoMapper;
using Continental.API.Core.Contracts.Responses;
using Continental.API.Core.Domain;
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
        CreateMap<CuentaCorriente, CuentaCorrienteResponse>();
    }
}