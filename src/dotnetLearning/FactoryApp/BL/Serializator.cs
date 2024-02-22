using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using dotnetLearning.FactoryApp.Model;

namespace dotnetLearning.FactoryApp.BL
{
    internal static class Serializator
    {
        internal static void SerializeDataJson(IEnumerable<Tank> tanks, IEnumerable<Unit> units, IEnumerable<Factory> factories)
         {
             var options = new JsonSerializerOptions
             {
                 Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                 WriteIndented = true
             };

             string jsonTanks = JsonSerializer.Serialize(tanks, options);
             string jsonUnits = JsonSerializer.Serialize(units, options);
             string jsonFactories = JsonSerializer.Serialize(factories, options);

             string filePath = @"C:\Users\user\source\repos\dotnetLearning\Lesson 1\";

             File.WriteAllText(filePath + "Tanks.json", jsonTanks);
             File.WriteAllText(filePath + "Units.json", jsonUnits);
             File.WriteAllText(filePath + "Factories.json", jsonFactories);
         }
    }
}
