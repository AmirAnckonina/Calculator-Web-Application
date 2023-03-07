using CalculatorWebApi.ApiServices.CalculatorServices;
using CalculatorWebApi.ApiServices.LogLevelServices;
using CalculatorWebApi.Loggers;
using CalculatorWebApi.Loggers.LoggersFactory;
using CalculatorWebApi.Loggers.LoggersService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls($"http://localhost:9583/");
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICalculatorLoggerService, CalculatorLoggerService>();
builder.Services.AddSingleton<ILoggerLevelService, LoggerLevelService>();
builder.Services.AddSingleton<ICalculatorLoggerFactory, CalculatorLoggerFactory>();
builder.Services.AddSingleton<ICalculatorService, CalculatorService>();
builder.Services.AddSingleton<ICalculatorStackService, CalculatorStackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestsLoggingMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
