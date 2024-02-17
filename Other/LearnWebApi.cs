using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Channels;

namespace dotnetLearning.Other
{
    public record class Todo(
    int? UserId = null,
    int? Id = null,
    string? Title = null,
    bool? Completed = null);

    internal static class LearnWebApi
    {
        #region fields
        private static HttpClient _httpClient = new() { BaseAddress = new Uri("https://jsonplaceholder.typicode.com") };
        #endregion

        #region methods
        internal static async Task Run()
        {
            await GetAsync(_httpClient, "todos?userId=1&completed=false");
            await GetAsync(_httpClient, "todos/3"); // крашится на преобразовании JSON в List<Todo> (по адресу JSON на 1 объект)
        }
        public static async Task GetAsync(HttpClient httpClient, string path)
        {
            // через расширение GetFromJsonAsync
            await Console.Out.WriteLineAsync("Выполняется через расширение GetFromJsonAsync:\n");
            var todos = await httpClient.GetFromJsonAsync<List<Todo>>(path);
            todos?.ForEach(Console.WriteLine);


            // через HttpResponseMessage.
            await Console.Out.WriteAsync("\nВыполняется через расширение HttpResponseMessage:\n");
            using HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                // Не могу понять почему не десериализует из потока json в List<ToDo> 
                var result = await JsonSerializer.DeserializeAsync<List<Todo>>(contentStream);
            }
            else
            {
                Console.WriteLine($"HTTP запрос неуспешен. Код: {response.StatusCode}");
            }
        }
        #endregion
    }
}