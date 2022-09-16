using Continental.API.Core.Contracts.Requests;

namespace Continental.API.Core.Contracts.Responses;

public record CuentaCorrienteResponse
{
    public string CuentaCompleta { get; set; }
    public SaldoDisponible Saldo { get; set; }
}

public record SaldoDisponible
{
    public decimal Saldo { get; init; }
    public decimal SaldoRetencion { get; init; }
    public decimal SaldoBloqueo { get; init; }
    public decimal SaldoCombinado { get; init; }
    public decimal SaldoPrincipal { get; init; }
    public decimal SaldoCombinadoFinal { get; init; }
}