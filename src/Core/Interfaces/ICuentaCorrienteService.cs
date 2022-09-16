using System.Threading.Tasks;
using Continental.API.Core.Domain;

namespace Continental.API.Core.Interfaces;

public interface ICuentaCorrienteService
{
    Task<CuentaCorriente> ConsultarCuentaConSaldo(string numeroCuenta);
}
