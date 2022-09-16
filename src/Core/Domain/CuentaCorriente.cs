using System;

namespace Continental.API.Core.Domain;

public class CuentaCorriente
{
    public string CuentaCompleta { get; private set; }
    public string Sucursal { get; private set; }
    public string Aplica { get; private set; }
    public string NumeroCuenta { get; private set; }
    public string SubCuenta { get; private set; }
    public SaldoDisponible? Saldo { get; private set; }

    public CuentaCorriente(string sucursal, string aplica, string numeroCuenta, string subCuenta, SaldoDisponible saldo)
    {
        if (string.IsNullOrEmpty(sucursal))
        {
            throw new ArgumentException("Sucursal no puede estar vacia", nameof(Sucursal));
        }

        Sucursal = sucursal;

        if (string.IsNullOrEmpty(aplica))
        {
            throw new ArgumentException("Aplica no puede estar vacia", nameof(Aplica));
        }

        Aplica = aplica;

        if (string.IsNullOrEmpty(numeroCuenta))
        {
            throw new ArgumentException("NumeroCuenta no puede estar vacia", nameof(NumeroCuenta));
        }

        NumeroCuenta = numeroCuenta;

        if (string.IsNullOrEmpty(subCuenta))
        {
            throw new ArgumentException("SubCuenta no puede estar vacia", nameof(SubCuenta));
        }

        SubCuenta      = subCuenta;
        CuentaCompleta = sucursal + aplica + numeroCuenta + subCuenta;
        Saldo          = saldo;
    }
}

public class SaldoDisponible
{
    public decimal Saldo { get; init; }
    public decimal SaldoRetencion { get; init; }
    public decimal SaldoBloqueo { get; init; }
    public decimal SaldoCombinado { get; init; }
    public decimal SaldoPrincipal => Saldo - SaldoRetencion - SaldoBloqueo;
    public decimal SaldoCombinadoFinal => SaldoPrincipal + SaldoCombinado;
}

public class CuentaCorrienteBuilder
{
    private string _sucursal { get; set; }
    private string _aplica { get; set; }
    private string _numeroCuenta { get; set; }
    private string _subCuenta { get; set; }

    private SaldoDisponible _saldo { get; set; }

    public CuentaCorrienteBuilder()
    {
    }

    public CuentaCorrienteBuilder(string sucursal, string aplica, string numeroCuenta, string subCuenta)
    {
        _sucursal     = sucursal;
        _aplica       = aplica;
        _numeroCuenta = numeroCuenta;
        _subCuenta    = subCuenta;
    }

    public CuentaCorrienteBuilder WithSucursal(string sucursal)
    {
        _sucursal = sucursal;

        return this;
    }

    public CuentaCorrienteBuilder WithAplica(string aplica)
    {
        _aplica = aplica;

        return this;
    }

    public CuentaCorrienteBuilder WithNumeroCuenta(string numeroCuenta)
    {
        _numeroCuenta = numeroCuenta;

        return this;
    }

    public CuentaCorrienteBuilder WithSubCuenta(string subCuenta)
    {
        _subCuenta = subCuenta;

        return this;
    }

    public CuentaCorrienteBuilder WithSaldo(SaldoDisponible saldo)
    {
        _saldo = saldo;

        return this;
    }

    public CuentaCorriente Build()
    {
        return new CuentaCorriente(_sucursal, _aplica, _numeroCuenta, _subCuenta, _saldo);
    }
}
