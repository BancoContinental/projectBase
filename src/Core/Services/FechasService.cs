using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;

namespace Continental.API.Core.Services
{
    public class FechasService : IFechasService
    {
        private readonly IFechasRepository _repository;

        public FechasService(IFechasRepository repository)
        {
            _repository = repository;
        }

        public async Task<DiaHabil> EsDiaHabil(DateTime fecha)
        {
            if (EsFinDeSemana(fecha))
            {
                return new DiaHabil
                {
                    Fecha   = fecha,
                    Mensaje = "NO"
                };
            }

            var esDiaHabil = await _repository.EsDiaHabil(fecha);

            var mensaje = esDiaHabil ? "SI" : "NO";

            return new DiaHabil
            {
                Fecha = fecha,
                Mensaje = mensaje
            };
        }

        private bool EsFinDeSemana(DateTime fecha)
        {
            return fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
