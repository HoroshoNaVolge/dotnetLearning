using dotnetLearning.Lesson1;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace dotnetLearning.Lesson_1
{
    internal class LegacyMethods
    {
        /// <summary>
        /// Тестовый метод создания объектов через оператор new
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tank> OldGetTanks()
        {
            // ваш код здесь
            Tank[] tanks =
            {
             new (1, 1500, 2000, "Резервуар 1", "Надземный-вертикальный", 1),
             new (2, 2500, 3000, "Резервуар 2", "Надземный-горизонтальный", 1),
             new (3, 3000, 3000, "Резервуар 3", "Надземный-горизонтальный", 2),
             new (4, 3000, 3000, "Резервуар 4", "Надземный-вертикальный", 2),
             new (5, 4000, 5000, "Резервуар 5", "Подземный-двустенный", 2),
             new (6, 500, 500, "Резервуар 6", "Подводный",3),
            };

            return tanks;
        }

        public static IEnumerable<Factory> OldGetFactories()
        {
            // ваш код здесь
            Factory factory1 = new(1, "НПЗ№1", "Первый нефтеперерабатывающий завод");
            Factory factory2 = new(2, "НПЗ№2", "Второй нефтеперерабатывающий завод");

            return [factory1, factory2];
        }

        public static IEnumerable<Unit> OldGetUnits()
        {
            // ваш код здесь
            Unit unit1 = new(1, "ГФУ-2", "Газофракционирующая установка", 1);
            Unit unit2 = new(2, "АВТ-6", "Атмосферно-вакуумная трубчатка", 1);
            Unit unit3 = new(3, "АВТ-10", "Атмосферно - вакуумная трубчатка", 2);
            return [unit1, unit2, unit3];
        }

        // Код запуска: SerializeDataJson(tanks, units, factories);
        private static void SerializeDataJson(IEnumerable<Tank> tanks, IEnumerable<Unit> units, IEnumerable<Factory> factories)
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
