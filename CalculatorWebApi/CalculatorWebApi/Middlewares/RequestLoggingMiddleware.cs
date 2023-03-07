using Serilog;
using CalculatorWebApi.Middlewares;
using CalculatorWebApi.Loggers;
using System.Reflection.Metadata;
using System.Diagnostics;
using CalculatorWebApi.Controllers;
using Serilog.Context;
using CalculatorWebApi.Loggers.LoggersService;
using LogEventLevel = Serilog.Events.LogEventLevel;

namespace CalculatorWebApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly ICalculatorLoggerService m_RequestsLogger;
        private static int s_RequestsCounter = 0;
        private readonly RequestDelegate m_Next;
        private readonly Stopwatch m_DurationTimer;

        public RequestLoggingMiddleware(RequestDelegate next, ICalculatorLoggerService calcLoggerService) 
        {
            m_RequestsLogger = calcLoggerService;
            m_Next = next;
            m_DurationTimer = new Stopwatch();
        }

        public Task Invoke(HttpContext context)
        {
            var resource = context.Request.Path;
            var httpMethod = context.Request.Method;

            ++s_RequestsCounter;
            using (LogContext.PushProperty("RequestNumber", s_RequestsCounter))
            {
                m_RequestsLogger.WriteLog(eLoggerType.Requests, LogEventLevel.Information,
                    $"Incoming request | #{s_RequestsCounter} | resource: {resource} | HTTP Verb {httpMethod}");

                m_DurationTimer.Restart();
                m_Next.Invoke(context);
                m_DurationTimer.Stop();

                m_RequestsLogger.WriteLog(eLoggerType.Requests, LogEventLevel.Debug,
                    $"request #{s_RequestsCounter} duration: {m_DurationTimer.ElapsedMilliseconds}ms");
            }

            return Task.CompletedTask;
        }
    }
}

public static class EndpointLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestsLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}

