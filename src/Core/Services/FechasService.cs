using System;
using System.Linq;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;

namespace Continental.API.Core.Services;

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
                Mensaje = OpcionesEnum.No.ToString()
            };
        }

        var esDiaHabil = await _repository.GetDiaHabil(fecha);

        var mensaje = _getMensaje(esDiaHabil);

        return new DiaHabil
        {
            Fecha   = fecha,
            Mensaje = mensaje.ToString()
        };
    }

    private OpcionesEnum _getMensaje(bool esDiaHabil) => esDiaHabil ? OpcionesEnum.Si : OpcionesEnum.No;

    public async Task<Feriado> GetFeriado(DateOnly fecha)
    {
        var feriados = await _repository.GetFeriado(fecha);

        if (feriados.Any())
        {
            return feriados.FirstOrDefault();
        }

        return default;
    }

    private static bool EsFinDeSemana(DateTime fecha)
    {
        return fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
}