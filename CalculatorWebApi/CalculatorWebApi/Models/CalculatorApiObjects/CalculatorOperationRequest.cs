using System.Text.Json.Serialization;

namespace CalculatorWebApi.Models.RequestResponseObjects
{
    public class CalculatorOperationRequest
    {
        [JsonPropertyName("arguments")]
        public int[]? Arguments { get; set; }

        [JsonPropertyName("operation")]
        public string? Operation { get; set; } 
    }
}
