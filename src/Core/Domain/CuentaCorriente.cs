using System;
using Continental.API.Core.Domain.Common;

namespace Continental.API.Core.Domain;

public class CuentaCorriente
{
    public string NumeroCuenta { get; private set; }
    public EstadoCuenta EstadoCuenta { get; }
    public decimal Saldo { get; private set; }

    public CuentaCorriente(string numeroCuenta, EstadoCuenta estadoCuenta, decimal saldo)
    {
        _withNumeroCuenta(numeroCuenta);
        EstadoCuenta = estadoCuenta;
        _withSaldo(saldo);
    }

    private void _withNumeroCuenta(string numeroCuenta)
    {
        if (numeroCuenta.Length > 14)
        {
            throw new ArgumentException(nameof(NumeroCuenta), "El numero de cuenta no puede ser mayor a 14 caracteres");
        }

        NumeroCuenta = numeroCuenta;
    }

    private void _withSaldo(decimal saldo)
    {
        if (saldo < 0)
        {
            throw new ArgumentException(nameof(Saldo), "El saldo no puede ser negativo");
        }

        Saldo = saldo;
    }
}
