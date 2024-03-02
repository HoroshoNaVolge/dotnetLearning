using dotnetLearning.FactoryApp.Model;
using dotnetLearning.FactoryApp.Service.SerializationService;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace dotnetLearning.FactoryApp.Service.FacilityService
{
    public interface IFacilityService
    {
        public Task AddDataToJsonAsync(IFacility facility, CancellationToken token);
        public Task CreateOrUpdateJsonDataAsync(CancellationToken token);
        public Task GetAllJsonDataAsync(string filePath, CancellationToken token);
        public Task DeleteDataFromJsonAsync(IFacility facility, CancellationToken token);
        public Task UpdateJsonDataAsync(IFacility facility, CancellationToken token);

        public Task CreateOrUpdateDataExcelAsync(CancellationToken token);
        public Task GetDataFromExcelAsync(CancellationToken token);
        public Task AddDataToExcelAsync(IFacility facility, CancellationToken token);
        public Task DeleteDataFromExcelAsync(IFacility facility, CancellationToken token);
        public Task UpdateDataExcelAsync(IFacility facility, CancellationToken token);

        public Task WriteAllToDbAsync(CancellationToken token);
        public Task AddFacilityDbAsync(IFacility facility, CancellationToken token);
        public Task UpdateFacilityDbAsync(IFacility facility, CancellationToken token);
        public Task DeleteFacilityDbAsync(IFacility facility, CancellationToken token);
        public Task GetFacilitiesDbAsync(CancellationToken token);

        /// <returns>Строка с количеством текущих объектов Factory, Unit,Tank</returns>
        public string? GetCurrentConfiguration();
        public string? GetTotalSummary();
        public string? GetTotalVolumeTanks();
        public string? GetTanksSummary();
        public string? GetFactoriesSummary();
        public string? GetUnitsSummary();

        public Unit? FindUnit(string tankName);
        public Factory? FindFactory(Unit unit);
        public Factory? FindFactory(string unitName);
        public IFacility? Search(string name);
    }

    public class FacilityService(IOptions<FacilityServiceOptions> options, JsonService jsonService, ExcelService excelService, DbFacilitiesService dbService) : IFacilityService
    {
        private FacilitiesContainer? container = null;
        private readonly JsonService? jsonService = jsonService;
        private readonly ExcelService? excelService = excelService;
        private readonly DbFacilitiesService? dbService = dbService;
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true,
        };

        private IList<Factory>? factories = null;
        private IList<Unit>? units = null;
        private IList<Tank>? tanks = null;

        private readonly string jsonFilePath = options.Value.FacilitiesJsonFilePath!;
        private readonly string excelFilePath = options.Value.FacilitiesExcelFilePath!;

        #region JsonSerialization
        public async Task CreateOrUpdateJsonDataAsync(CancellationToken token)
        {
            if (container is null || factories is null || tanks is null || units is null) return;

            container = new(factories, units, tanks);

            //await js.CreateOrUpdateAllAsync(container, token);
            string json = JsonSerializer.Serialize(container, jsonSerializerOptions);

            using var writer = new StreamWriter(jsonFilePath);
            await writer.WriteAsync(json);
        }

        public async Task AddDataToJsonAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null) return;

            await jsonService!.AddFacilityAsync(facility, token);
        }

        public async Task GetAllJsonDataAsync(string filePath, CancellationToken token)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            var deserializedContainer = await JsonSerializer.DeserializeAsync<FacilitiesContainer>(stream, cancellationToken: token) ?? throw new ArgumentException("Ошибка десериализация в Facilities Container");

            factories = deserializedContainer.Factories;
            units = deserializedContainer.Units;
            tanks = deserializedContainer.Tanks;
        }

        public async Task DeleteDataFromJsonAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null) return;

            await jsonService!.DeleteFacilityAsync(facility, token);
        }

        public async Task UpdateJsonDataAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null) return;

            await jsonService!.UpdateFacilityAsync(facility, token);
        }

        #endregion

        #region ExcelSerialization
        public async Task CreateOrUpdateDataExcelAsync(CancellationToken token)
        {
            if (excelService is null || container is null) return;

            await excelService.CreateOrUpdateAllAsync(container, token);
        }
        public async Task GetDataFromExcelAsync(CancellationToken token)
        {
            if (excelService is null) return;

            if (container is null)
#nullable disable
                container = new(factories, units, tanks);
#nullable restore
            await excelService.GetFacilitiesAsync(container, token);

            factories = container.Factories;
            units = container.Units;
            tanks = container.Tanks;
        }

        public async Task AddDataToExcelAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null || excelService is null) return;

            if (tanks is null || factories is null || units is null)
                await GetAllJsonDataAsync(jsonFilePath, token);

            bool anyAdded = false;
            switch (facility)
            {
                case Tank tank when !tanks?.Any(tank => tank.Id == facility.Id) == true:
                    tanks?.Add(tank);
                    anyAdded = true;
                    break;
                case Factory factory when !factories?.Any(fac => fac.Id == facility.Id) == true:
                    factories?.Add(factory);
                    anyAdded = true;
                    break;
                case Unit unit when !units?.Any(u => u.Id == facility.Id) == true:
                    units?.Add(unit);
                    anyAdded = true;
                    break;
            }

            if (!anyAdded || factories is null || units is null || tanks is null) return;

            container = new(factories, units, tanks);

            await excelService.CreateOrUpdateAllAsync(container, token);
        }

        public async Task DeleteDataFromExcelAsync(IFacility facility, CancellationToken token)
        {
            if (facility.GetType() == typeof(Factory))
                factories?.Remove((Factory)facility);

            if (facility.GetType() == typeof(Unit))
                units?.Remove((Unit)facility);

            if (facility.GetType() == typeof(Tank))
                tanks?.Remove((Tank)facility);

            if (factories is null || units is null || tanks is null) return;
            await CreateOrUpdateDataExcelAsync(token);
        }

        public async Task UpdateDataExcelAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null) return;

            await excelService!.UpdateFacilityAsync(facility, token);
        }
        #endregion

        #region GetInfoAsString
        public string? GetFactoriesSummary() => factories != null ? string.Join(Environment.NewLine, factories) : null;

        public string? GetUnitsSummary() => units != null ? string.Join(Environment.NewLine, units) : null;

        public string? GetTanksSummary() => tanks != null ? string.Join(Environment.NewLine, tanks) : null;

        public string? GetCurrentConfiguration() { return $"Количество резервуаров: {tanks?.Count ?? 0}, установок: {units?.Count ?? 0}"; }

        public string? GetTotalSummary()
        {
            if (factories is null || units is null || tanks is null) return null;

            // Наверное не стоит кастить к обджекту, просто хотел сделал лакончино.
            var items = factories.Cast<object>().Concat(units).Concat(tanks);

            return $"По состоянию на {DateTime.Now} в работе:" + Environment.NewLine +
                     string.Join(Environment.NewLine, items.Select(item => item.ToString()));
        }

        public string? GetTotalVolumeTanks()
        {
            if (tanks != null)
                return $"Общая заполненность резервуаров, мт: {tanks?.Sum(tank => tank.Volume)}";
            return null;
        }
        #endregion

        #region Searches
        public Unit? FindUnit(string tankName) =>
            units?.FirstOrDefault(u => tanks?.Any(t => t.Name == tankName && t.UnitId == u.Id) ?? false);

        // Считаю нужным перегрузить на приём строки параметром, т.к. работа с моделью не происходит вне класса FacilityService.
        public Factory? FindFactory(string unitName)
        {
            return units?.FirstOrDefault(unit => unit.Name == unitName)?.FactoryId is var factoryId
                        ? factories?.FirstOrDefault(factory => factory.Id == factoryId)
                        : null;
        }
        // Оставляю реализацию т.к. указана в задании.
        public Factory? FindFactory(Unit unit) =>
            factories?.FirstOrDefault(f => f.Id == unit.Id);

        public IFacility? Search(string searchName)
        {
            if (container is null)
                if (factories is null || units is null || tanks is null) return null;
                else container = new(factories, units, tanks);

            var factoryResult = container.Factories
                .FirstOrDefault(Factory => Factory.Name == searchName);

            if (factoryResult != default)
                return factoryResult;

            var unitResult = container.Units
                .FirstOrDefault(Unit => Unit.Name == searchName);

            if (unitResult != default)
                return unitResult;

            var tankResult = container.Tanks.
                FirstOrDefault(Tank => Tank.Name == searchName);
            return tankResult != default ? tankResult : null;
        }
        #endregion

        #region DataBaseCRUD
        public async Task WriteAllToDbAsync(CancellationToken token)
        {
            if (container is null || dbService is null)
                return;
            await dbService.CreateOrUpdateAllAsync(container, token);
        }
        public async Task AddFacilityDbAsync(IFacility facility, CancellationToken token)
        {
            if (dbService is null)
                return;
            await dbService.AddFacilityAsync(facility, token);
        }
        public async Task UpdateFacilityDbAsync(IFacility facility, CancellationToken token)
        {
            if (dbService is null)
                return;
            await dbService.UpdateFacilityAsync(facility, token);
        }
        public async Task DeleteFacilityDbAsync(IFacility facility, CancellationToken token)
        {
            if (dbService is null)
                return;
            await dbService.DeleteFacilityAsync(facility, token);
        }
        public async Task GetFacilitiesDbAsync(CancellationToken token)
        {
            if (dbService is null) return;

            if (container is null)
#nullable disable
                container = new(factories, units, tanks);
#nullable restore
            await dbService.GetFacilitiesAsync(container, token);

            factories = container.Factories;
            units = container.Units;
            tanks = container.Tanks;
        }
        #endregion
    }
}