using AspNetCore.WebApi.Models;
using System.Text.Json;

namespace AspNetCore.WebApi.Services
{
    public interface IDadataService
    {
        Task<QueryOrganizationResult> GetOrganizationNameByInnAsync(string inn, CancellationToken token);
    }

    public class DadataService : IDadataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DadataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<QueryOrganizationResult> GetOrganizationNameByInnAsync(string inn, CancellationToken token)
        {
            var httpClient = _httpClientFactory.CreateClient("DadataClient");
            var response = await httpClient.GetAsync($"?query={inn}");

            if (!response.IsSuccessStatusCode)
                return new() { IsSuccess = false, ErrorDescription = "Ошибка запроса к API", OrganizationName = null };

            else
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var result = JsonSerializer.Deserialize<QueryOrganizationResult>(stream);

                // тут ошибка. Смотреть в suggestions, 
                if (result.suggestions.Count() == 0)
                    return new() { IsSuccess = false, ErrorDescription = $"Не найдена организация с ИНН {inn}", OrganizationName = null };
                return new() { IsSuccess = true, OrganizationName = result.suggestions.FirstOrDefault()?.value };

            }

        }
    }
}
