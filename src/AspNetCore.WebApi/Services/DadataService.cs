using System.Text.Json;

namespace AspNetCore.WebApi.Services
{
    public interface IDadataService
    {
        Task<string> GetOrganizationNameByInnAsync(string inn);
    }

    public class DadataService : IDadataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DadataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string?> GetOrganizationNameByInnAsync(string inn)
        {
            var httpClient = _httpClientFactory.CreateClient("DadataClient");
            var response = await httpClient.GetAsync($"?query={inn}");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var jsonDocument = await JsonDocument.ParseAsync(stream);
                var root = jsonDocument.RootElement;

                var suggestionsArray = root.GetProperty("suggestions").EnumerateArray();
                try
                {
                    var firstSuggestion = suggestionsArray.FirstOrDefault();
                    return firstSuggestion.GetProperty("value").GetString();
                }
                catch { return null; }
            }

            return $"HTTP запрос не успешен. Код: {response.StatusCode}";
        }
    }
}
