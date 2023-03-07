using Serilog.Core;

namespace CalculatorWebApi.Loggers.LoggersFactory
{
    public interface ICalculatorLoggerFactory
    {
        Serilog.ILogger BuildLogger(eLoggerType loggerType, LoggingLevelSwitch loggerSwitch);
    }
}
