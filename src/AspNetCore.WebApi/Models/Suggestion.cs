using System.Text.Json.Serialization;

namespace AspNetCore.WebApi.Models
{
    internal class Suggestion
    {
        [JsonPropertyName("suggestions")]
        public Suggestion[]? Suggestions { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
