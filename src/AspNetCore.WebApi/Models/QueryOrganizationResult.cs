using System.Text.Json.Serialization;

namespace AspNetCore.WebApi.Models
{
    public class QueryOrganizationResult
    {
        [JsonPropertyName("suggestions")]
        public Suggestion[]? Suggestions { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorDescription { get; set; }
        public string? OrganizationName { get; set; }
    }
    
}
