using Microsoft.AspNetCore.Mvc;

namespace SerilogTest.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger) {
      _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get() {
      _logger.LogTrace("Trace"); // Verbose
      _logger.LogDebug("Debug");
      _logger.LogInformation("Information");
      _logger.LogWarning("Warning");
      _logger.LogError("Error");
      _logger.LogCritical("Critical"); // Fatal

      var weatherForecastResult = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = Random.Shared.Next(-20, 55),
          Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

      _logger.LogInformation("Information {weatherForecastResult}", weatherForecastResult[0]);
      _logger.LogInformation("Information {@weatherForecastResult}", weatherForecastResult[0]);
      return weatherForecastResult;
    }

    [HttpGet("/GetWeatherForecastException")]
    public IEnumerable<WeatherForecast> GetException() { 
      throw new AggregateException("TestException");
    }
  }
}