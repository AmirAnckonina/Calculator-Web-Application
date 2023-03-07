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
    public class StackCalculatorController : ControllerBase
    {
        private readonly ICalculatorStackService m_CalcStack;
        private readonly ICalculatorLoggerService m_StackLogger;

        public StackCalculatorController(ICalculatorLoggerService calcLoggerService,
            ICalculatorStackService calcStackService) 
        {
            m_StackLogger = calcLoggerService;
            m_CalcStack =  calcStackService;
        }

        [HttpGet("/stack/size")]
        public ActionResult<CalculatorResponse> GetCalculatorStackSize()
        {
            CalculatorResponse stackSizeResp = new CalculatorResponse();
            stackSizeResp.Result = m_CalcStack.GetCalcStackSize();

            m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Information,
                $"Stack size is {stackSizeResp.Result}");

            m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Debug,
                $"Stack content (first == top): " +
                $"[{string.Join(", ", m_CalcStack.GetStackContent())}]");

            return Ok(stackSizeResp);
        }

        [HttpDelete("/stack/arguments")]
        public ActionResult<CalculatorResponse> DeleteNumbersFromStack(int count)
        {
            CalculatorResponse delResp = new CalculatorResponse();

            try
            {
                m_CalcStack.DeleteArgumentsFromStack(count);
                delResp.Result = m_CalcStack.GetCalcStackSize();

                m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Information,
                    $"Removing total {count} argument(s) from the stack | " +
                    $"Stack size: {m_CalcStack.GetCalcStackSize()}");

                return Ok(delResp);
            }
            catch (InvalidOperationException)
            {
                delResp.ErrorMessage = $"Error: cannot remove {count} from the stack." +
                    $" It has only {m_CalcStack.GetCalcStackSize()} arguments";

                m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Error, delResp.ErrorMessage);

                return Conflict(delResp);
            }

        }

        [HttpPut("/stack/arguments")]
        public ActionResult<CalculatorResponse> AddArgumentsToStack(AddArgumentsRequest addArgReq)
        {
            CalculatorResponse calcResp = new CalculatorResponse();

            int stackSizeBefore = m_CalcStack.GetCalcStackSize();
            m_CalcStack.AddArgumentsToStack(addArgReq.Arguments);
            calcResp.Result = m_CalcStack.GetCalcStackSize();

            m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Information,
                $"Adding total of {(addArgReq.Arguments != null ? addArgReq.Arguments.Count() : 0)} " +
                $"argument(s) to the stack | " +
                $"Stack size: {m_CalcStack.GetCalcStackSize()}");

            m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Debug,
                $"Adding arguments: " +
                $"{string.Join(",", (addArgReq.Arguments != null ? addArgReq.Arguments : Array.Empty<int>()))} | " +
                $"Stack size before {stackSizeBefore} | stack size after {calcResp.Result}");

            return Ok(calcResp);
        }

        [HttpGet("/stack/operate")]
        public ActionResult<CalculatorResponse> ExecuteCalculationByStack(
            [FromQuery(Name = "operation")] string operation)
        {
            CalculatorResponse calcResp = new CalculatorResponse();

            try
            {
               (int[] poppedNums, calcResp.Result) = m_CalcStack.Calculate(operation);
               int stackSize = m_CalcStack.GetCalcStackSize();

                m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Information,
                    $"Performing operation {operation}. " +
                    $"Result is {calcResp.Result} | " +
                    $"stack size: {stackSize}");

                m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Debug,
                    $"Performing operation: {operation}({string.Join(",", poppedNums)}) = {calcResp.Result}");

               return Ok(calcResp);
            }
            catch (NotEnoughArgumentsException ex)
            {
                calcResp.ErrorMessage = $"Error: cannot implement operation {ex.OperationName}. " +
                    $"It requires {ex.RequiredArguments} arguments," +
                    $" and the stack has only {m_CalcStack.GetCalcStackSize()} arguments";

                m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Error, calcResp.ErrorMessage);

                return Conflict(calcResp);
            }
            catch (Exception ex)
            {
                calcResp.ErrorMessage = ex.Message;
                m_StackLogger.WriteLog(eLoggerType.Stack, LogEventLevel.Error, calcResp.ErrorMessage);

                return Conflict(calcResp);
            }
        }
    }
}
