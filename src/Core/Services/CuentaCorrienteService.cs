using System.Threading.Tasks;
using Continental.API.Core.Domain;
using Continental.API.Core.Interfaces;

namespace Continental.API.Core.Services;

public class CuentaCorrienteService : ICuentaCorrienteService
{
    private readonly ICuentaCorrienteRepository _cuentaCorrienteRepository;
    private readonly ISaldoService _saldoService;

    public CuentaCorrienteService(ICuentaCorrienteRepository cuentaCorrienteRepository, ISaldoService saldoService)
    {
        _cuentaCorrienteRepository = cuentaCorrienteRepository;
        _saldoService              = saldoService;
    }

    public async Task<CuentaCorriente> ConsultarCuentaConSaldo(string numeroCuenta)
    {
        var cuenta = await _cuentaCorrienteRepository.ObtenerPorNumeroCuenta(numeroCuenta);

        if (cuenta is null)
        {
            return default;
        }

        var saldo = await _saldoService.ObtenerSaldo(cuenta.CuentaCompleta);

        var builder = new CuentaCorrienteBuilder()
            .WithSucursal(cuenta.Sucursal)
            .WithAplica(cuenta.Aplica)
            .WithNumeroCuenta(cuenta.NumeroCuenta)
            .WithSubCuenta(cuenta.SubCuenta)
            .WithSaldo(new SaldoDisponible
            {
                Saldo          = saldo.SaldoDisponible.Saldo,
                SaldoBloqueo   = saldo.SaldoDisponible.SaldoBloqueo,
                SaldoCombinado = saldo.SaldoDisponible.SaldoCombinado,
                SaldoRetencion = saldo.SaldoDisponible.SaldoRetencion
            });

        return builder.Build();
    }
}
