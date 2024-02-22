using System.Text.Json.Serialization;

namespace AspNetCore.WebApi.Models
{
    internal class SuggestionResponse
    {
        [JsonPropertyName("suggestions")]
        public IEnumerable<Suggestion>? Suggestions { get; set; }
    }
    internal class Suggestion
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
