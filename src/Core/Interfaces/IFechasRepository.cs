using System;
using System.Threading.Tasks;

namespace Continental.API.Core.Interfaces
{
    public interface IFechasRepository
    {
        Task<bool> EsDiaHabil(DateTime fecha);
    }
}