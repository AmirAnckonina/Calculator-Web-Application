using LogEventLevel = Serilog.Events.LogEventLevel;

namespace CalculatorWebApi.Loggers.LoggersService
{
    public interface ICalculatorLoggerService
    {
        public void WriteLog(eLoggerType logger, LogEventLevel logLevel, string content);
        public void SetLoggerLevel(eLoggerType logger, LogEventLevel newLoglevel);
        public LogEventLevel GetLoggerLevel(eLoggerType logger);
    }
}
