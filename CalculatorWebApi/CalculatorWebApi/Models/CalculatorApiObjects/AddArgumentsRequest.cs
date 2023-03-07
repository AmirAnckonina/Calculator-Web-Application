using System.Text.Json.Serialization;

namespace CalculatorWebApi.Models.RequestResponseObjects
{
    public class AddArgumentsRequest
    {
        [JsonPropertyName("arguments")]
        public int[]? Arguments { get; set; }
    }
}
