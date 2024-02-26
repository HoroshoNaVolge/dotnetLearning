using dotnetLearning.FactoryApp.Model;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace dotnetLearning.FactoryApp.Service.FacilityService
{
    public interface IFacilityService
    {
        public Task SerializeDataJsonAsync(IList<Factory> factories, IList<Unit> units, IList<Tank> tanks, CancellationToken token);
        public Task SerializeDataJsonAsync(IFacility facility, CancellationToken token);
        public Task SerializeDataJsonAsync(CancellationToken token);
        public Task DeserializeDataJson(string filePath);

        public Task ExportDataToExcelAsync(CancellationToken token);
        public Task ImportDataFromExcelAsync(CancellationToken token);


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
    }

    public class FacilityService : IFacilityService
    {
        private FacilitiesContainer? container = null;
        private ExcelTransformator? excelTransformator = null;
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true,
        };

        private IList<Factory>? factories = null;
        private IList<Unit>? units = null;
        private IList<Tank>? tanks = null;

        private readonly string _jsonFilePath;

        #region Serialization
        public FacilityService(IOptions<FacilityServiceOptions> options, ExcelTransformator excelTransformator)
        {
            if (string.IsNullOrEmpty(options.Value.FacilitiesJsonFilePath))
                throw new ArgumentException("В конфигурации ошибка в JsonFilePath");

            _jsonFilePath = options.Value.FacilitiesJsonFilePath;

            this.excelTransformator = excelTransformator;
        }

        //Для сериализации в учебных условиях. Например тестовые объекты через new в коде C#
        public async Task SerializeDataJsonAsync(IList<Factory> factories, IList<Unit> units, IList<Tank> tanks, CancellationToken token)
        {
            container = new(factories, units, tanks);

            string json = JsonSerializer.Serialize(container, jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);
            await writer.WriteAsync(json);
        }

        // Может понадобиться сериализовать всю текущую конфигурацию
        public async Task SerializeDataJsonAsync(CancellationToken token)
        {
            if (container is null || factories is null || tanks is null || units is null) return;

            container = new(factories, units, tanks);

            string json = JsonSerializer.Serialize(container, jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);
            await writer.WriteAsync(json);
        }

        public async Task SerializeDataJsonAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null || factories is null || units is null || tanks is null) return;

            bool anyAdded = false;
            switch (facility)
            {
                case Tank tank when !tanks?.Any(tank => tank.Id == facility.Id) == true:
                    tanks?.Add(tank);
                    anyAdded = true;
                    break;
                case Factory factory when !factories.Any(fac => fac.Id == facility.Id) == true:
                    factories?.Add(factory);
                    anyAdded = true;
                    break;
                case Unit unit when !units.Any(u => u.Id == facility.Id) == true:
                    units?.Add(unit);
                    anyAdded = true;
                    break;
            }

            if (!anyAdded || factories is null || tanks is null || units is null) return;

            container = new(factories, units, tanks);

            string json = JsonSerializer.Serialize(container, jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);

            await writer.WriteAsync(json);
        }

        public async Task DeserializeDataJson(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            var deserializedContainer = await JsonSerializer.DeserializeAsync<FacilitiesContainer>(stream) ?? throw new ArgumentException("Ошибка десериализация в Facilities Container");

            factories = deserializedContainer.Factories;
            units = deserializedContainer.Units;
            tanks = deserializedContainer.Tanks;
        }
        #endregion

        #region GetInfoAsString
        public string? GetFactoriesSummary() => factories != null ? string.Join(Environment.NewLine, factories) : null;

        public string? GetUnitsSummary() => units != null ? string.Join(Environment.NewLine, units) : null;

        public string? GetTanksSummary() => tanks != null ? string.Join(Environment.NewLine, tanks) : null;

        public string? GetCurrentConfiguration() { return $"Количество резервуаров: {tanks?.Count()}, установок: {units?.Count()}"; }

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

        public async Task ExportDataToExcelAsync(CancellationToken token)
        {
            if (excelTransformator is null || factories is null || units is null || tanks is null) return;

            await excelTransformator.WriteToExcelAsync(factories, units, tanks);
        }

        public async Task ImportDataFromExcelAsync(CancellationToken token)
        {
            if (excelTransformator != null)
                await excelTransformator.GetFacilitiesFromExcel();
        }
    }
}