using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using dotnetLearning.Other;

namespace dotnetLearning.Lesson2
{
    internal static class JsonParser
    {
        public static IEnumerable<Deal> GetJsonFromFile(string path)
        {

            string fileContent = File.ReadAllText(path);
            IEnumerable<Deal> deserializedDeals = JsonSerializer.Deserialize<List<Deal>>(fileContent);
            return deserializedDeals;
        }
    }
}
