using System;
using System.Globalization;
using System.Threading.Tasks;
using Continental.API.Core.Interfaces;
using Continental.API.Core.Services;
using Moq;
using NUnit.Framework;

namespace Core.UnitTests.Services
{
    [TestFixture]
    public class FechasServiceTests
    {
        private Mock<IFechasRepository> _repository;
        private FechasService _service;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IFechasRepository>();
            _service = new FechasService(_repository.Object);
        }

        [Test]
        public async Task EsDiaHabil_CuandoEsFinDeSemana_RetornaNo()
        {
            // Generamos un Sabado
            var sabado = (int)DayOfWeek.Saturday;
            var hoy    = DateTime.Today;
            var fecha  = hoy.AddDays(sabado - (int) hoy.DayOfWeek);

            var resultado = await _service.EsDiaHabil(fecha);

            Assert.That(resultado.Mensaje.ToUpper(), Is.EqualTo("NO"));
        }

        [Test]
        public async Task EsDiaHabil_CuandoEsFeriado_RetornaNo()
        {
            var feriado   = DateTime.ParseExact(
                $"25/12/{DateTime.Now.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture);

            var resultado = await _service.EsDiaHabil(feriado);

            Assert.That(resultado.Mensaje.ToUpper(), Is.EqualTo("NO"));
        }

        [Test]
        public async Task EsDiaHabil_CuandoEsDiaHabil_RetornaSi()
        {
            // Generamos un Viernes
            var sabado = (int)DayOfWeek.Saturday;
            var hoy    = DateTime.Today;
            var fecha  = hoy.AddDays(sabado - (int)hoy.DayOfWeek - 1);

            _repository.Setup(r
                => r.EsDiaHabil(fecha, null))
                .Returns(Task.FromResult(true));

            var resultado = await _service.EsDiaHabil(fecha);

            Assert.That(resultado.Mensaje.ToUpper(), Is.EqualTo("SI"));
        }
    }
}
