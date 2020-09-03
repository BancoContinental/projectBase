using System;
using System.Threading.Tasks;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class HomeController : BaseApiController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFechasService _service;

        public HomeController(ILogger<HomeController> logger, IFechasService service)
        {
            _logger = logger;
            _service     = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Hola mundo");
        }

        [HttpGet("{diaHabil}")]
        public async Task<IActionResult> DiaHabil(DateTime diaHabil)
        {
            _logger.LogInformation("Consultando dia habil {0}", diaHabil.Date);

            try
            {
                var resultado = await _service.EsDiaHabil(diaHabil);
                var respuesta = Mapper.Map<DiaHabil>(resultado);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al consultar el dia habil");

                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
        }
    }
}
