using System.Net.Http.Json;
using System.Text.Json;

namespace dotnetLearning.Other
{
    public record class Todo(
    int? UserId = null,
    int? Id = null,
    string? Title = null,
    bool? Completed = null);

    internal static class LearnWebApi
    {
        internal static void Run()
        {
            HttpClient client = GetHttpClient("https://jsonplaceholder.typicode.com");
            GetAsync(client).Wait();
            GetFromJsonAsync(client, "todos?userId=1&completed=false").Wait();
        }

        private static HttpClient? _httpClient;

        public static HttpClient GetHttpClient(string uri)
        {

            if (_httpClient == null)
            {
                return _httpClient = new() { BaseAddress = new Uri(uri) };
            }
            return _httpClient;
        }

        public static async Task GetFromJsonAsync(HttpClient httpClient, string path)
        {
            // через расширение
            var todos = await httpClient.GetFromJsonAsync<List<Todo>>(path);

            // по классике через HttpResponseMessage
            using HttpResponseMessage response = await httpClient.GetAsync(path);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            List<Todo>? todos2 = JsonSerializer.Deserialize<List<Todo>>(jsonResponse);
            todos?.ForEach(Console.WriteLine);


        }
        public static async Task GetAsync(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("todos/3");

            try { response.EnsureSuccessStatusCode(); }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
    }


}

