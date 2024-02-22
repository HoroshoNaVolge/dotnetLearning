namespace AspNetCore.WebApi.Services
{
    public class DadataServiceOptions
    {
        public const string SectionName = "Dadata";
        public string? DaDataApiBaseUrl { get; set; }
        public string? DaDataApiToken { get; set; }
    }
}
