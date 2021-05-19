using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;

namespace Search.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IElasticClient _elasticClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IElasticClient elasticClient)
        {
            _logger = logger;
            _elasticClient = elasticClient;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post(WeatherForecast weatherForecast)
        {
            var a = 12;
            // var weather = new WeatherForecast()
            // {
            //     Date = DateTime.Now,
            //     TemperatureC = 23,
            //     Summary = "Warm"
            // };

            var response = await _elasticClient.IndexDocumentAsync(weatherForecast);

            return Ok(response.DebugInformation);
        }
        
        [HttpGet("get-all-weather-test")]
        public async Task<IActionResult> GetAll()
        {
            var response = _elasticClient.Search<WeatherForecast>();

            return Ok(response.Documents);
        }
        
        
    }
}