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
        private FacilitiesContainer? _container = null;
        private ExcelTransformator? _excelTransformator = null;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true,
        };

        private IList<Factory>? _factories = null;
        private IList<Unit>? _units = null;
        private IList<Tank>? _tanks = null;

        private readonly string _jsonFilePath;

        #region Serialization
        public FacilityService(IOptions<FacilityServiceOptions> options, ExcelTransformator excelTransformator)
        {
            if (string.IsNullOrEmpty(options.Value.FacilitiesJsonFilePath))
                throw new ArgumentException("В конфигурации ошибка в JsonFilePath");

            _jsonFilePath = options.Value.FacilitiesJsonFilePath;

            _excelTransformator = excelTransformator;
        }

        //Для сериализации в учебных условиях. Например тестовые объекты через new в коде C#
        public async Task SerializeDataJsonAsync(IList<Factory> factories, IList<Unit> units, IList<Tank> tanks, CancellationToken token)
        {
            _container = new(factories, units, tanks);

            string json = JsonSerializer.Serialize(_container, _jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);
            await writer.WriteAsync(json);
        }

        // Может понадобиться сериализовать всю текущую конфигурацию
        public async Task SerializeDataJsonAsync(CancellationToken token)
        {
            if (_container is null || _factories is null || _tanks is null || _units is null) return;

            _container = new(_factories, _units, _tanks);

            string json = JsonSerializer.Serialize(_container, _jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);
            await writer.WriteAsync(json);
        }

        public async Task SerializeDataJsonAsync(IFacility facility, CancellationToken token)
        {
            if (facility is null || _factories is null || _units is null || _tanks is null) return;

            bool anyAdded = false;
            switch (facility)
            {
                case Tank tank when !_tanks?.Any(tank => tank.Id == facility.Id) == true:
                    _tanks?.Add(tank);
                    anyAdded = true;
                    break;
                case Factory factory when !_factories.Any(fac => fac.Id == facility.Id) == true:
                    _factories?.Add(factory);
                    anyAdded = true;
                    break;
                case Unit unit when !_units.Any(u => u.Id == facility.Id) == true:
                    _units?.Add(unit);
                    anyAdded = true;
                    break;
            }

            if (!anyAdded || _factories is null || _tanks is null || _units is null) return;

            _container = new(_factories, _units, _tanks);

            string json = JsonSerializer.Serialize(_container, _jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);

            await writer.WriteAsync(json);
        }

        public async Task DeserializeDataJson(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            var deserializedContainer = await JsonSerializer.DeserializeAsync<FacilitiesContainer>(stream) ?? throw new ArgumentException("Ошибка десериализация в Facilities Container");

            _factories = deserializedContainer.Factories;
            _units = deserializedContainer.Units;
            _tanks = deserializedContainer.Tanks;
        }
        #endregion

        #region GetInfoAsString
        public string? GetFactoriesSummary() => _factories != null ? string.Join(Environment.NewLine, _factories) : null;

        public string? GetUnitsSummary() => _units != null ? string.Join(Environment.NewLine, _units) : null;

        public string? GetTanksSummary() => _tanks != null ? string.Join(Environment.NewLine, _tanks) : null;

        public string? GetCurrentConfiguration() { return $"Количество резервуаров: {_tanks?.Count()}, установок: {_units?.Count()}"; }

        public string? GetTotalSummary()
        {
            if (_factories is null || _units is null || _tanks is null) return null;

            // Наверное не стоит кастить к обджекту, просто хотел сделал лакончино.
            var items = _factories.Cast<object>().Concat(_units).Concat(_tanks);

            return $"По состоянию на {DateTime.Now} в работе:" + Environment.NewLine +
                     string.Join(Environment.NewLine, items.Select(item => item.ToString()));
        }

        public string? GetTotalVolumeTanks()
        {
            if (_tanks != null)
                return $"Общая заполненность резервуаров, мт: {_tanks?.Sum(tank => tank.Volume)}";
            return null;
        }
        #endregion

        public Unit? FindUnit(string tankName) =>
            _units?.FirstOrDefault(u => _tanks?.Any(t => t.Name == tankName && t.UnitId == u.Id) ?? false);

        // Считаю нужным перегрузить на приём строки параметром, т.к. работа с моделью не происходит вне класса FacilityService.
        public Factory? FindFactory(string unitName)
        {
            return _units?.FirstOrDefault(unit => unit.Name == unitName)?.FactoryId is var factoryId
                        ? _factories?.FirstOrDefault(factory => factory.Id == factoryId)
                        : null;
        }
        // Оставляю реализацию т.к. указана в задании.
        public Factory? FindFactory(Unit unit) =>
            _factories?.FirstOrDefault(f => f.Id == unit.Id);

        public async Task ExportDataToExcelAsync(CancellationToken token)
        {
            if (_excelTransformator is null || _factories is null || _units is null || _tanks is null) return;

            await _excelTransformator.WriteToExcelAsync(_factories, _units, _tanks);
        }
    }
}