using System.Text.Json.Serialization;

namespace AspNetCore.WebApi.Models
{
    public class QueryOrganizationResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorDescription { get; set; }
        public string? OrganizationName { get; set; }
    }

}
