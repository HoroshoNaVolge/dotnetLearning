using Microsoft.Extensions.Options;
using dotnetLearning.FactoryApp.Service.FacilityService;
using dotnetLearning.FactoryApp.View;

namespace dotnetLearning.FactoryApp.Service
{
    public class FactoryAppService(IFacilityService facilityService, IView view, IOptions<FacilityServiceOptions> options)
    {
        internal async Task RunAsync()
        {
            if (options.Value.FacilitiesJsonFilePath is not null)
                await facilityService.DeserializeDataJson(options.Value.FacilitiesJsonFilePath);
            else
                throw new ArgumentNullException("Ошибка в файле конфигурации appsettings.json");

            view.ShowMessage(facilityService.GetCurrentConfiguration());
            view.ShowMessage(facilityService.GetTotalSummary());
            view.ShowMessage(facilityService.GetTotalVolumeTanks());


            // Комменты ниже допилю позже, также доп.задачи к первым урокам 

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