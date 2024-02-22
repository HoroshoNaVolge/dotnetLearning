using AspNetCore.WebApi.Models;
using System.Text.Json;

namespace AspNetCore.WebApi.Services
{
    public interface IDadataService
    {
        Task<QueryOrganizationResult> GetOrganizationNameByInnAsync(string inn, CancellationToken token);
    }

    public class DadataService(IHttpClientFactory httpClientFactory) : IDadataService
    {
        public async Task<QueryOrganizationResult> GetOrganizationNameByInnAsync(string inn, CancellationToken token)
        {
            var httpClient = httpClientFactory.CreateClient("DadataClient");
            var response = await httpClient.GetAsync($"?query={inn}", token);

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return new() { IsSuccess = false, ErrorDescription = "В запросе использован невалидный API токен", OrganizationName = null };

            using var stream = await response.Content.ReadAsStreamAsync(token);
            var result = JsonSerializer.Deserialize<SuggestionResponse>(stream);

            var item = result?.Suggestions?.FirstOrDefault()?.Value;
            if (item is null)
                return new() { IsSuccess = false, ErrorDescription = $"Не найдена организация с ИНН {inn}", OrganizationName = null };
            return new() { IsSuccess = true, OrganizationName = item };
        }
    }
}