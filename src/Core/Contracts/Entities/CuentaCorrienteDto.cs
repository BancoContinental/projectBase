namespace Continental.API.Core.Contracts.Entities;

public class CuentaCorrienteDto
{
    public string CuentaCompleta { get; set; }
    public string Sucursal { get; set; }
    public string Aplica { get; set; }
    public string NumeroCuenta { get; set; }
    public string SubCuenta { get; set; }
    public int Cancel { get; set; }
}