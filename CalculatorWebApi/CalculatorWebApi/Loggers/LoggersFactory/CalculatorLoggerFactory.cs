using Serilog;
using Serilog.Core;

namespace CalculatorWebApi.Loggers.LoggersFactory
{
    public class CalculatorLoggerFactory : ICalculatorLoggerFactory
    {
        private static readonly string LOGS_DIR =
            $"{Environment.CurrentDirectory}/logs";

        private const string CALC_OUTPUT_TEPMPLATE =
            "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} {CustomLogLevel}: {Message} | request #{RequestNumber} {NewLine}";

        private Serilog.ILogger BuildRequestsLogger(LoggingLevelSwitch loggerSwitch)
        {
            Serilog.ILogger reqLogger;

            reqLogger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggerSwitch)
                .WriteTo.Console(outputTemplate: CALC_OUTPUT_TEPMPLATE)
                .WriteTo.File($"{LOGS_DIR}/requests.log", outputTemplate: CALC_OUTPUT_TEPMPLATE)
                .Enrich.With<CustomLogLevelEnricher>()
                .Enrich.FromLogContext()
                .CreateLogger();

            return reqLogger;
        }

        private Serilog.ILogger BuildStackLogger(LoggingLevelSwitch loggerSwitch)
        {
            Serilog.ILogger stackLogger;

            stackLogger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggerSwitch)
                .WriteTo.File($"{LOGS_DIR}/stack.log", outputTemplate: CALC_OUTPUT_TEPMPLATE)
                .Enrich.With<CustomLogLevelEnricher>()
                .Enrich.FromLogContext()
                .CreateLogger();

            return stackLogger;
        }

        private Serilog.ILogger BuildIndependentLogger(LoggingLevelSwitch loggerSwitch)
        {
            Serilog.ILogger indLogger;

            indLogger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggerSwitch)
                .WriteTo.File($"{LOGS_DIR}/independent.log", outputTemplate: CALC_OUTPUT_TEPMPLATE)
                .Enrich.With<CustomLogLevelEnricher>()
                .Enrich.FromLogContext()
                .CreateLogger();

            return indLogger;
        }

        public Serilog.ILogger BuildLogger(eLoggerType loggerType , LoggingLevelSwitch loggerSwitch)
        {
            Serilog.ILogger logger;

            switch (loggerType)
            {
                case eLoggerType.Requests:
                    logger = BuildRequestsLogger(loggerSwitch);
                    break;

                case eLoggerType.Independent:
                    logger = BuildIndependentLogger(loggerSwitch);
                    break;

                case eLoggerType.Stack:
                    logger = BuildStackLogger(loggerSwitch);
                    break;

                default:
                    logger = new LoggerConfiguration().CreateLogger();
                    break;
            }

            return logger;
        }
    }
}

