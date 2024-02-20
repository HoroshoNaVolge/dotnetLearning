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
            var response = await httpClient.GetAsync($"?query={inn}", token);

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)

                return new() { IsSuccess = false, ErrorDescription = "В запросе использован невалидный API токен", OrganizationName = null };

            else
            {
                using var stream = await response.Content.ReadAsStreamAsync(token);
                var result = JsonSerializer.Deserialize<QueryOrganizationResult>(stream);

                if (result == null || result?.Suggestions?.Count() == 0)
                    return new() { IsSuccess = false, ErrorDescription = $"Не найдена организация с ИНН {inn}", OrganizationName = null };
                return new() { IsSuccess = true, OrganizationName = result?.Suggestions?.FirstOrDefault()?.Value };
            }
        }
    }
}