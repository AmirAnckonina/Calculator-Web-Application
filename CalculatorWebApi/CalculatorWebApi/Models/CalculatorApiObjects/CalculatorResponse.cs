using System.Text.Json.Serialization;

namespace CalculatorWebApi.Models.RequestResponseObjects
{
    public class CalculatorResponse
    {
        [JsonPropertyName("result")]
        public int? Result { get; set; }

        [JsonPropertyName("error-message")]
        public string? ErrorMessage { get; set; }
    }
}
