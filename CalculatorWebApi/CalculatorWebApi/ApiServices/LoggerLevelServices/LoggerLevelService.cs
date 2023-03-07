using CalculatorWebApi.Loggers;
using CalculatorWebApi.Loggers.LoggersService;
using Serilog.Events;

namespace CalculatorWebApi.ApiServices.LogLevelServices
{
    public class LoggerLevelService : ILoggerLevelService
    {
        private readonly ICalculatorLoggerService m_LoggersService;

        public LoggerLevelService(ICalculatorLoggerService loggersService)
        {
            m_LoggersService = loggersService;
        }
        private LogEventLevel GetLogLevelByInput(string logLevel)
        {
            LogEventLevel logEventLevel;

            switch (logLevel)
            {
                case "INFO":
                    logEventLevel = LogEventLevel.Information;
                    break;

                case "ERROR":
                    logEventLevel = LogEventLevel.Error;
                    break;

                case "DEBUG":
                    logEventLevel = LogEventLevel.Debug;
                    break;

                default:
                    throw new Exception($"the loggers service couldn't find the log level: {logLevel}");
            }

            return logEventLevel;
        }

        private eLoggerType GetLoggerByInput(string loggerName)
        {
            eLoggerType logger;

            switch (loggerName)
            {
                case "requests-logger":
                    logger = eLoggerType.Requests;
                    break;

                case "stack-logger":
                    logger = eLoggerType.Stack;
                    break;

                case "independent-logger":
                    logger = eLoggerType.Independent;
                    break;

                default:
                    throw new Exception($"the loggers service couldn't find {loggerName}");
            }

            return logger;
        }

        public void SetLoggerLevel(string loggerName, string logLevel)
        {
            eLoggerType logger = GetLoggerByInput(loggerName);
            LogEventLevel logEventLevel = GetLogLevelByInput(logLevel);

            m_LoggersService.SetLoggerLevel(logger, logEventLevel);
        }

        public string GetLoggerLevel(string loggerName)
        {
            string currentLogLevel;

            eLoggerType logger = GetLoggerByInput(loggerName);
            LogEventLevel logLevel = m_LoggersService.GetLoggerLevel(logger);

            currentLogLevel = logLevel.ToString().ToUpper();
            if (logLevel == LogEventLevel.Information)
            {
                currentLogLevel = currentLogLevel.Substring(0, 4);
            }

            return currentLogLevel;
        }
    }
}
