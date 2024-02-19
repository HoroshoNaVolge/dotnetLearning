namespace AspNetCore.WebApi.Models
{
    public class QueryOrganizationResult
    {
        public Suggestion[]? suggestions { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorDescription { get; set; }
        public string? OrganizationName { get; set; }
    }
    public class Suggestion
    {
        public string? value { get; set; }
    }
}
