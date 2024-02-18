using System.Text.Json;

namespace dotnetLearning.Other
{
    public record class Todo(
    int? userId = null,
    int? id = null,
    string? title = null,
    bool? completed = null);

    internal static class LearnWebApi
    {
        #region fields
        private static HttpClient _httpClient = new() { BaseAddress = new Uri("https://jsonplaceholder.typicode.com") };
        #endregion

        #region methods
        internal static async Task Run()
        {
            await GetAsync<Todo>(_httpClient, "todos?userId=1&completed=false");

            await GetAsync<Todo>(_httpClient, "todos/3");
        }

        // Заменил все методы на дженерик для сокращения кода и удобства вызова
        public static async Task GetAsync<T>(HttpClient httpClient, string path)
        {
            var response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();

                JsonDocument jsonDocument = await JsonDocument.ParseAsync(stream);

                if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
                {
                    // ревайнд потока для возможности десериализации с начала
                    stream.Seek(0, SeekOrigin.Begin);

                    await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<T>(stream))
                        await Console.Out.WriteLineAsync(item?.ToString());
                }
                else
                {
                    // ревайнд потока для возможности десериализации с начала
                    stream.Seek(0, SeekOrigin.Begin);

                    var singleItem = await JsonSerializer.DeserializeAsync<T>(stream);
                    await Console.Out.WriteLineAsync(singleItem?.ToString());
                }
            }
            else { await Console.Out.WriteLineAsync($"HTTP запрос неуспешен. Код: {response.StatusCode}"); return; }
        }
        #endregion
    }
}
