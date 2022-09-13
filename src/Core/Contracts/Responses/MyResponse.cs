using Continental.API.Core.Contracts.Requests;

namespace Continental.API.Core.Contracts.Responses;

public class MyResponse
{
    public MyRequest CuentaCorriente { get; set; }
    public string Mensaje { get; set; }
}