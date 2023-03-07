using CalculatorWebApi.ApiServices.LogLevelServices;
using CalculatorWebApi.Loggers.LoggersService;
using CalculatorWebApi.Models.RequestResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggersCalculatorController : ControllerBase
    {
        private readonly ILoggerLevelService m_LogLevelService;

        public LoggersCalculatorController(ILoggerLevelService logLevelService)
        {
            m_LogLevelService = logLevelService;
        }

        [HttpGet("/logs/level")]
        public ActionResult<CalculatorResponse> GetLogLevel(
            [FromQuery(Name = "logger-name")] string loggerName)
        {
            string outputMessage;

            try
            {
                string logLevel = m_LogLevelService.GetLoggerLevel(loggerName);
                outputMessage = $"Success: {logLevel.ToUpper()}";

                return Ok(outputMessage);
            }
            catch (Exception ex)
            {
                outputMessage = $"Failure: {ex.Message}";

                return BadRequest(outputMessage);
            }

        }

        [HttpPut("/logs/level")]
        public ActionResult<CalculatorResponse> SetLogLevel(
            [FromQuery(Name = "logger-name")] string loggerName,
            [FromQuery(Name = "logger-level")] string logLevel)
        {
            string outputMessage;

            try
            {
                m_LogLevelService.SetLoggerLevel(loggerName, logLevel);
                outputMessage = $"Success: {logLevel.ToUpper()}";
                
                return Ok(outputMessage);
            }
            catch (Exception ex) 
            {
                outputMessage = $"Failure: {ex.Message}";

                return BadRequest(outputMessage);
            }
        }
    }
}
