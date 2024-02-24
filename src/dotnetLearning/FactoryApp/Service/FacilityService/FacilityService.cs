using dotnetLearning.FactoryApp.Model;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;

namespace dotnetLearning.FactoryApp.Service.FacilityService
{
    public interface IFacilityService
    {
        public Task SerializeDataJsonAsync(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks, CancellationToken token);
        public Task DeserializeDataJson(string filePath);

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
        private JsonContainer? _container = null;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };

        private IEnumerable<Factory>? _factories = null;
        private IEnumerable<Unit>? _units = null;
        private IEnumerable<Tank>? _tanks = null;

        private readonly string _jsonFilePath;


        public FacilityService(IOptions<FacilityServiceOptions> options)
        {
            if (options.Value.FacilitiesJsonFilePath is null)
                throw new ArgumentException("В конфигурации ошибка в JsonFilePath");
            _jsonFilePath = options.Value.FacilitiesJsonFilePath;
        }

        public async Task SerializeDataJsonAsync(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks, CancellationToken token)
        {
            _container = new(factories, units, tanks);

            string json = JsonSerializer.Serialize(_container, _jsonSerializerOptions);

            using var writer = new StreamWriter(_jsonFilePath);
            await writer.WriteAsync(json);
        }

        public async Task DeserializeDataJson(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            var deserializedContainer = await JsonSerializer.DeserializeAsync<JsonContainer>(stream) ?? throw new ArgumentException("Ошибка десериализация в JsonContainer");
            _factories = deserializedContainer.Factory;
            _units = deserializedContainer.Unit;
            _tanks = deserializedContainer.Tank;
        }

        public string? GetFactoriesSummary() => _factories != null ? string.Join(Environment.NewLine, _factories) : null;

        public string? GetUnitsSummary() => _units != null ? string.Join(Environment.NewLine, _units) : null;

        public string? GetTanksSummary() => _tanks != null ? string.Join(Environment.NewLine, _tanks) : null;

        public string? GetCurrentConfiguration() { return $"Количество резервуаров: {_tanks?.Count()}, установок: {_units?.Count()}"; }

        public string? GetTotalSummary()
        {
            if (_factories is null || _units is null || _tanks is null) return null;

            var totalSummary = string.Empty;

            totalSummary += $"По состоянию на {DateTime.Now} в работе:\n";
            foreach (var factory in _factories)
                totalSummary += factory.ToString() + Environment.NewLine;
            foreach (var unit in _units)
                totalSummary += unit.ToString() + Environment.NewLine;
            foreach (var tank in _tanks)
                totalSummary += tank.ToString() + Environment.NewLine;

            return totalSummary;
        }

        public string? GetTotalVolumeTanks()
        {
            if (_tanks != null)
                return $"Общая заполненность резервуаров, мт: {_tanks?.Sum(tank => tank.Volume)}";
            return null;
        }

        public Unit? FindUnit(string tankName) =>
            _units?.FirstOrDefault(u => _tanks?.Any(t => t.Name == tankName && t.UnitId == u.Id) == true);

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
    }
}