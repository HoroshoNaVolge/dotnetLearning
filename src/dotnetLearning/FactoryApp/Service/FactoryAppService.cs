using System.Text.Json;
using dotnetLearning.FactoryApp.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using dotnetLearning.FactoryApp.Service.FacilityService;

namespace dotnetLearning.FactoryApp.Service
{
    public class FactoryAppService(IFacilityService facilityService)
    {
        internal void Run()
        {
             facilityService.SerializeDataJson(new List<Factory>() { new Factory(1, "SDADSDAS", "test"), new Factory(2, "fact2", "test") }
                , new List<Unit>() { new Unit(1, "fact", "test", 1), new Unit(2, "fact2", "test", 2) }
                , new List<Tank>() { new(1, 1000, 2000, "tank", "xiy", 2) });


            //var tanks = GetTanks(@"\Json\Tanks.json");
            //var units = GetUnits(@"C:\Units.json");
            //var factories = GetFactories(@"C:\Factories.json");

            //Console.WriteLine($"Количество резервуаров: {tanks.Count()}, установок: {units.Count()}");

            //var foundUnit = FindUnit(units, tanks, "Резервуар 2");
            //var factory = FindFactory(factories, foundUnit);

            //Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

            //var totalVolume = GetTotalVolume(tanks);
            //Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

            //foreach (var tank in tanks)
            //{
            //    Console.WriteLine($"{tank.Name}, {tank.Description}, Номер установки: {tank.UnitId}");
            //}

            //foreach (var unit in units)
            //{
            //    Console.WriteLine($"{unit.Name}, {unit.Description}, Номер фабрики: {unit.FactoryId}");
            //}

            //foreach (var fact in factories)
            //{
            //    Console.WriteLine($"{fact.Name}, {fact.Description}");
            //}
        }


        // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
        // можно использовать создание объектов прямо в C# коде через new, или читать из файла (на своё усмотрение)
        //public static IEnumerable<Tank> GetTanks(string jsonPath)
        //{
        //    string json = File.ReadAllText(jsonPath);
        //    IEnumerable<Tank>? tanks = JsonSerializer.Deserialize<Tank[]>(json);
        //    return tanks ?? Enumerable.Empty<Tank>();
        //}

        //// реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
        //public static IEnumerable<Unit> GetUnits(string jsonPath)
        //{
        //    string json = File.ReadAllText(jsonPath);
        //    IEnumerable<Unit>? units = JsonSerializer.Deserialize<Unit[]>(json);
        //    return units ?? Enumerable.Empty<Unit>();
        //}

        //// реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
        //public static IEnumerable<Factory> GetFactories(string jsonPath)
        //{
        //    string json = File.ReadAllText(jsonPath);
        //    IEnumerable<Factory>? factories = JsonSerializer.Deserialize<Factory[]>(json);
        //    return factories ?? Enumerable.Empty<Factory>();
        //}

        //// реализуйте этот метод, чтобы он возвращал установку (Unit), которой
        //// принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
        //// учтите, что по заданному имени может быть не найден резервуар
        //public static Unit? FindUnit(IEnumerable<Unit> units, IEnumerable<Tank> tanks, string tankName)
        //{
        //    var foundUnit = from tank in tanks
        //                    where tank.Name == tankName
        //                    join u in units on tank.UnitId equals u.Id
        //                    select new Unit(u.Id, u.Name, u.Description, u.FactoryId);
        //    return foundUnit.First();
        //}

        //// реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
        //public static Factory? FindFactory(IEnumerable<Factory> factories, Unit unit) => factories.FirstOrDefault(factory => factory.Id == unit.Id);

        //// реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
        //public static int GetTotalVolume(IEnumerable<Tank> tanks) => tanks.Sum(tank => tank.Volume);

    }
}