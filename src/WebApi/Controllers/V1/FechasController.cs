using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Controllers.V1;

public class FechasController : BaseApiController
{
    private readonly ILogger<FechasController> _logger;
    private readonly IFechasService _service;

    public FechasController(ILogger<FechasController> logger, IFechasService service)
    {
        _logger  = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Hola mundo");
    }

    [HttpGet("{diaHabil}")]
    public async Task<IActionResult> DiaHabil(DateTime diaHabil)
    {
        _logger.LogInformation("Consultando dia habil {Dia}", diaHabil.Date);

        try
        {
            var resultado = await _service.EsDiaHabil(diaHabil);

            _logger.LogDebug("Respuesta de consulta {@Respuesta}", resultado);

            var respuesta = Mapper.Map<DiaHabil>(resultado);

            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error al consultar el dia habil");

            return BadRequest($"Ocurrio un error: {ex.Message}");
        }
    }

    [HttpGet("feriado/{fecha}")]
    public async Task<IActionResult> GetFeriado([FromRoute]DateTime fecha)
    {
        _logger.LogInformation("Consultando feriado {Fecha}", fecha.Date);

        try
        {
            var respuesta = await _service.GetFeriado(DateOnly.FromDateTime(fecha));

            if (respuesta is null)
            {
                return NoContent();
            }

            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error al consultar el feriado");

            return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrio un error: {ex.Message}");
        }
    }
}