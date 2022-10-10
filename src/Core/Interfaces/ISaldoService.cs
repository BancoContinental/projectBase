using System.Threading.Tasks;
using Continental.API.Core.Contracts.Entities;

namespace Continental.API.Core.Interfaces;

public interface ISaldoService
{
    Task<SaldoCuenta> ObtenerSaldo(string cuenta);
}
