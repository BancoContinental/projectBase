namespace Continental.API.Core.Contracts.Entities;

public record SaldoCuenta
{
    public string NumeroCuenta { get; set; }
    public SaldoDisponible SaldoDisponible { get; set; }
}

public record SaldoDisponible
{
    public decimal Saldo { get; set; }
    public decimal SaldoRetencion { get; set; }
    public decimal SaldoBloqueo { get; set; }
    public decimal SaldoCombinado { get; set; }
}