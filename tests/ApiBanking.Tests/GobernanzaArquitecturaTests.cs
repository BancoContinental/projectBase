using Continental.API.Core;
using Continental.API.WebApi.Controllers;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace ApiBanking.Tests;

public class GobernanzaArquitecturaTests
{
    [Fact]
    public void Core_No_Depende_De_Infrastructure()
    {
        var types = Types.InAssembly(typeof(Install).Assembly);

        var result = types
            .That()
            .ResideInNamespace("Continental.API.Core")
            .ShouldNot()
            .HaveDependencyOn("Continental.API.Infrastructure")
            .GetResult().IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void Core_No_Depende_De_WebApi()
    {
        var types = Types.InAssembly(typeof(Install).Assembly);

        var result = types
            .That()
            .ResideInNamespace("Continental.API.Core")
            .ShouldNot()
            .HaveDependencyOn("Continental.API.WebApi")
            .GetResult().IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void Controllers_No_Dependen_Directamente_De_Infrastructure()
    {
        var types = Types.InAssembly(typeof(BaseApiController).Assembly);

        var result = types
            .That()
            .ResideInNamespace("Continental.API.WebApi.Controllers")
            .ShouldNot()
            .HaveDependencyOn("Continental.API.Infrastructure")
            .GetResult().IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_No_Dependen_De_WebApi()
    {
        var types = Types.InAssembly(typeof(Continental.API.Infrastructure.Install).Assembly);

        var result = types
            .That()
            .ResideInNamespace("Continental.API.Infrastructure")
            .ShouldNot()
            .HaveDependencyOn("Continental.API.WebApi")
            .GetResult().IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void Core_No_Tiene_Dependencias_Innecesarias()
    {
        var result = Types.InCurrentDomain()
            .That().HaveDependencyOnAny(new [] {
                "Microsoft.EntityFrameworkCore",
                "Oracle.ManagedDataAccess.Client",
                "MongoDB.Driver",
                "Dapper"
            }).And().ResideInNamespace("Continental.API")
            .Should()
            .ResideInNamespace("Continental.API.Infrastructure")
            .GetResult().IsSuccessful;

        result.Should().BeTrue();
    }
}