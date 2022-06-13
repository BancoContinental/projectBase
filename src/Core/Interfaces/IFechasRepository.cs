using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Continental.API.Core.Entities;

namespace Continental.API.Core.Interfaces
{
    public interface IFechasRepository
    {
        Task<bool> GetDiaHabil(DateTime fecha);
        Task<List<Feriado>> GetFeriado(DateOnly fecha);
    }
}