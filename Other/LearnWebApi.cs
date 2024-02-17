using System.Data;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Channels;

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
            await GetAsyncMultipleObjects(_httpClient, "todos?userId=1&completed=false");

            await GetAsyncSingleObject(_httpClient, "todos/m,l");
        }

        public static async Task GetAsyncSingleObject(HttpClient httpClient, string path)
        {
            // через расширение GetFromJsonAsync
            await Console.Out.WriteLineAsync("Выполняется получение 1 объекта через расширение GetFromJsonAsync:\n");

            var singleTodo = await httpClient.GetFromJsonAsync<Todo>(path);
            await Console.Out.WriteLineAsync(singleTodo?.ToString());

            // через HttpResponseMessage.
            await Console.Out.WriteAsync("\nВыполняется получение 1 объекта через HttpResponseMessage:\n");

            using HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<Todo>(contentStream);

                await Console.Out.WriteLineAsync(result?.ToString());
            }
            else
            {
                Console.WriteLine($"HTTP запрос неуспешен. Код: {response.StatusCode}");
            }
        }

        public static async Task GetAsyncMultipleObjects(HttpClient httpClient, string path)
        {
            // через расширение GetFromJsonAsync
            await Console.Out.WriteLineAsync("Выполняется получение множества объектов через расширение GetFromJsonAsync:\n");

            var todos = await httpClient.GetFromJsonAsync<List<Todo>>(path);
            todos?.ForEach(Console.WriteLine);

            // через HttpResponseMessage.
            await Console.Out.WriteAsync("\nВыполняется получение множества объектов через HttpResponseMessage:\n");

            using HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<List<Todo>>(contentStream);

                result?.ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine($"HTTP запрос неуспешен. Код: {response.StatusCode}");
            }
        }
          //Не работает пока. Буду разбираться.
          public static async Task<T> DeserializeJsonAsync<T>(Stream stream)
          {
              using (StreamReader reader = new StreamReader(stream))
              {
                  string jsonContent = await reader.ReadToEndAsync();

                  JsonDocument jsonDocument = JsonDocument.Parse(jsonContent);

                  if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
                  {

                      List<T> resultList = JsonSerializer.Deserialize<List<T>>(jsonContent);


                      return resultList.FirstOrDefault() ?? default;
                  }
                  else
                  {
                      try
                      {
                          T result = JsonSerializer.Deserialize<T>(jsonContent);
                          return result;
                      }
                      catch (JsonException)
                      {
                          return default;
                      }
                  }
              }
          }
        #endregion
    }
}
