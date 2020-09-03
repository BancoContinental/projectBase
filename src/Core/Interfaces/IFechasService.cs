using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;

namespace Continental.API.Core.Interfaces
{
    public interface IFechasService
    {
        Task<DiaHabil> EsDiaHabil(DateTime fecha);
    }
}
