using CalculatorWebApi.Loggers;
using CalculatorWebApi.Exceptions;
using CalculatorWebApi.Models.RequestResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CalculatorWebApi.Loggers.LoggersService;
using LogEventLevel = Serilog.Events.LogEventLevel;
using CalculatorWebApi.ApiServices.CalculatorServices;

namespace CalculatorWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndependentCalculatorController : ControllerBase
    {
        private readonly ICalculatorLoggerService m_IndCalcLogger;
        private readonly ICalculatorService m_CalculatorUnit;

        public IndependentCalculatorController(ICalculatorLoggerService calcLoggersService,
            ICalculatorService calcService)
        {
            m_IndCalcLogger = calcLoggersService; 
            m_CalculatorUnit = calcService;
        }

        [HttpPost("/independent/calculate")]
        public ActionResult<CalculatorResponse> ExecuteCalculation(CalculatorOperationRequest calcReq)
        {
            CalculatorResponse calcResp = new CalculatorResponse();

            try
            {
                calcResp.Result = m_CalculatorUnit.Calculate(calcReq.Arguments, calcReq.Operation);
              
                m_IndCalcLogger.WriteLog(eLoggerType.Independent, LogEventLevel.Information,
                    $"Performing operation {calcReq.Operation}. Result is {calcResp.Result}");
 
                m_IndCalcLogger.WriteLog(eLoggerType.Independent, LogEventLevel.Debug,
                    $"Performing operation: {calcReq.Operation}" +
                    $"({string.Join(",", (calcReq.Arguments != null ? calcReq.Arguments : Array.Empty<int>()))})" +
                    $" = {calcResp.Result}");
              
                return Ok(calcResp);
            }
            catch (NotEnoughArgumentsException ex)
            {
                calcResp.ErrorMessage = $"Error: Not enough arguments to perform the operation {ex.OperationName}";
                m_IndCalcLogger.WriteLog(eLoggerType.Independent, LogEventLevel.Error, calcResp.ErrorMessage);
                return Conflict(calcResp);
            }
            catch (Exception ex)
            {
                calcResp.ErrorMessage = ex.Message;
                m_IndCalcLogger.WriteLog(eLoggerType.Independent, LogEventLevel.Error, calcResp.ErrorMessage);
                return Conflict(calcResp);
            }
        }
    }
}
