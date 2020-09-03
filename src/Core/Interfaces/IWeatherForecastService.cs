using System.Collections.Generic;
using Continental.API.Core.Entities;

namespace Continental.API.Core.Interfaces
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecast();
    }
}
