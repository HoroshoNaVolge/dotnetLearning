using System.Text.Json.Serialization;

namespace AspNetCore.WebApi.Models
{
    public class Suggestion
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
