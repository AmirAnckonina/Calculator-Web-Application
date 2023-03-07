namespace CalculatorWebApi.ApiServices.LogLevelServices
{
    public interface ILoggerLevelService
    {
        void SetLoggerLevel(string loggerLevel, string logLevel);
        string GetLoggerLevel(string loggerName);
    }
}
