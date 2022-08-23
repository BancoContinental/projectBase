using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Continental.API.Core.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Core.UnitTests.Services;

[TestFixture]
public class FechasServiceTests
{
    private Mock<IFechasSistemaRepository> _repository;
    private FechasService _service;

    [SetUp]
    public void SetUp()
    {
        _repository = new Mock<IFechasSistemaRepository>();
        _service    = new FechasService(_repository.Object);
    }

    [Test]
    public async Task EsDiaHabil_CuandoEsFinDeSemana_RetornaNo()
    {
        // Generamos un Sabado
        var sabado = (int)DayOfWeek.Saturday;
        var hoy    = DateTime.Today;
        var fecha  = hoy.AddDays(sabado - (int) hoy.DayOfWeek);

        var resultado = await _service.EsDiaHabil(fecha);

        resultado.Mensaje.ToUpper().Should().Be("NO");
    }

    [Test]
    public async Task EsDiaHabil_CuandoEsFeriado_RetornaNo()
    {
        var feriado   = DateTime.ParseExact(
            $"25/12/{DateTime.Now.Year}",
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture);

        var resultado = await _service.EsDiaHabil(feriado);

        resultado.Mensaje.ToUpper().Should().Be("NO");
    }

    [Test]
    public async Task EsDiaHabil_CuandoEsDiaHabil_RetornaSi()
    {
        // Generamos un Viernes
        var sabado = (int)DayOfWeek.Saturday;
        var hoy    = DateTime.Today;
        var fecha  = hoy.AddDays(sabado - (int)hoy.DayOfWeek - 1);

        _repository.Setup(r
                => r.GetDiaHabil(fecha))
            .Returns(Task.FromResult(true));

        var resultado = await _service.EsDiaHabil(fecha);

        resultado.Mensaje.ToUpper().Should().Be("SI");
    }

    [Test]
    public async Task GetFeriado_CuandoFechaEsFeriado_RetornaFeriadoConDiadDeSemana()
    {
        var fecha = new DateOnly(DateTime.Now.Year, 6, 12);
        _repository.Setup(r
                => r.GetFeriado(fecha))
            .Returns(Task.FromResult(new List<Feriado>
            {
                new Feriado
                {
                    Fecha = fecha.ToDateTime(TimeOnly.MinValue),
                    Dia   = ((DayOfWeek)fecha.Day).ToString()
                }
            }));

        var resultado = await _service.GetFeriado(fecha);

        resultado.Should().NotBeNull();
        resultado.Should().BeOfType<Feriado>();
        resultado.Fecha.Should().Be(fecha.ToDateTime(TimeOnly.MinValue));
    }

    [Test]
    public async Task GetFeriado_CuandoFechaNoEsFeriado_RetornaNull()
    {
        var fecha = new DateOnly(DateTime.Now.Year, 6, 13);
        _repository.Setup(r
                => r.GetFeriado(fecha))
            .Returns(Task.FromResult(new List<Feriado>()));

        var resultado = await _service.GetFeriado(fecha);

        resultado.Should().BeNull();
    }
}