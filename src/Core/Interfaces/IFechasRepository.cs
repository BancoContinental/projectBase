using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;

namespace Continental.API.Core.Interfaces
{
    public interface IFechasRepository
    {
        Task<bool> EsDiaHabil(DateTime fecha, CredencialesFinansys credenciales = null);
    }
}