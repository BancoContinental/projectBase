using System.Collections.Generic;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class WeatherForecastController : BaseApiController
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger  = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Consultando pronostico");

            return _service.GetForecast();
        }
    }
}
