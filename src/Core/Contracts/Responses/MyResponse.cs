using System;
using Continental.API.Core.Contracts.Requests;
using Continental.API.Core.Domain.Common;

namespace Continental.API.Core.Contracts.Responses;

public class MyResponse
{
    public MyRequest CuentaCorriente { get; set; }
    public string Mensaje { get; set; }
}