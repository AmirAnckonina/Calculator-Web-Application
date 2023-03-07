using System.Text.Json.Serialization;

namespace CalculatorWebApi.Models.CalculatorApiObjects
{
    public class SetLoggerLevelRequest
    {
        [JsonPropertyName("logger-name")]
        public string? Logger { get; set; }

        [JsonPropertyName("logger-level")]
        public string? Level { get; set; }
    }
}
