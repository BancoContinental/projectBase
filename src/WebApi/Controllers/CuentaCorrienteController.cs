using System;
using System.Threading.Tasks;
using Continental.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Controllers;

public class CuentaCorrienteController : BaseApiController
{
    private readonly ILogger<CuentaCorrienteController> _logger;
    private readonly ICuentaCorrienteService _cuentaCorrienteService;

    public CuentaCorrienteController(ILogger<CuentaCorrienteController> logger,
        ICuentaCorrienteService cuentaCorrienteService)
    {
        _logger                 = logger;
        _cuentaCorrienteService = cuentaCorrienteService;
    }

    [HttpGet("{cuenta}/saldo")]
    public async Task<IActionResult> Get([FromRoute] string cuenta)
    {
        return Ok(await _cuentaCorrienteService.ConsultarCuentaConSaldo(cuenta));
    }
}