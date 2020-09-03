using System;
using System.Collections.Generic;
using System.Linq;
using Continental.API.Core.Entities;
using Continental.API.Core.Interfaces;

namespace Continental.API.Core.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> GetForecast()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date         = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary      = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}
