using CalculatorWebApi.Loggers.LoggersFactory;
using Serilog.Core;
using LogEventLevel = Serilog.Events.LogEventLevel;

namespace CalculatorWebApi.Loggers.LoggersService
{
    public class CalculatorLoggerService : ICalculatorLoggerService
    {
        private readonly ICalculatorLoggerFactory m_LoggerFactory;
        private readonly Dictionary<eLoggerType, LoggingLevelSwitch> m_LogLevelSwitches;
        private readonly Dictionary<eLoggerType, Serilog.ILogger> m_Loggers;

        public CalculatorLoggerService(ICalculatorLoggerFactory calcLoggerFactory)
        {
            m_LoggerFactory = calcLoggerFactory;
            m_LogLevelSwitches = new Dictionary<eLoggerType, LoggingLevelSwitch>();
            m_Loggers = new Dictionary<eLoggerType, Serilog.ILogger>();
            CreateSwitches();
            CreateLoggers();
        }

        private void CreateSwitches()
        {
            m_LogLevelSwitches.Add(eLoggerType.Requests, new LoggingLevelSwitch(LogEventLevel.Information));
            m_LogLevelSwitches.Add(eLoggerType.Stack, new LoggingLevelSwitch(LogEventLevel.Information));
            m_LogLevelSwitches.Add(eLoggerType.Independent, new LoggingLevelSwitch(LogEventLevel.Debug));
        }

        private void CreateLoggers()
        {
            m_Loggers.Add(
                eLoggerType.Requests,
                m_LoggerFactory.BuildLogger(
                    eLoggerType.Requests, m_LogLevelSwitches[eLoggerType.Requests]));

            m_Loggers.Add(eLoggerType.Stack,
                m_LoggerFactory.BuildLogger(
                    eLoggerType.Stack, m_LogLevelSwitches[eLoggerType.Stack]));

            m_Loggers.Add(eLoggerType.Independent,
                m_LoggerFactory.BuildLogger(
                    eLoggerType.Independent, m_LogLevelSwitches[eLoggerType.Independent]));
        }

        public void WriteLog(eLoggerType logger, LogEventLevel logLevel, string content)
        {
            switch (logLevel)
            {
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                    m_Loggers[logger].Write(logLevel, content);
                    break;

                case LogEventLevel.Error:
                    m_Loggers[logger].Write(logLevel, $"Server encountered an error ! message: " + content);
                    break;

                default:
                    break;
            }
        }

        public void SetLoggerLevel(eLoggerType logger, LogEventLevel newLogLevel)
        {
            m_LogLevelSwitches[logger].MinimumLevel = newLogLevel;
        }

        public LogEventLevel GetLoggerLevel(eLoggerType logger)
        {
            return m_LogLevelSwitches[logger].MinimumLevel;
        }
    }
}
