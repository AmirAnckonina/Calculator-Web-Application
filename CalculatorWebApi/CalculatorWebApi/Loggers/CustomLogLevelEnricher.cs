using Serilog.Core;
using Serilog.Events;

namespace CalculatorWebApi.Loggers
{
    public class CustomLogLevelEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string customLogLevel = string.Empty;

            switch(logEvent.Level)
            {
                case LogEventLevel.Information:
                    customLogLevel = logEvent.Level.ToString().ToUpper().Substring(0,4);
                    break;
                default:
                    customLogLevel = logEvent.Level.ToString().ToUpper();
                    break;
            }

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CustomLogLevel", customLogLevel));
        }
    }
}
