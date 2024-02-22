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
    }

    public class FacilityService : IFacilityService
    {
        private IEnumerable<Factory>? _factories = null;
        private IEnumerable<Unit>? _units = null;
        private IEnumerable<Tank>? _tanks = null;

        private readonly string _jsonFilePath;
        private JsonContainer? container = null;

        public FacilityService(IOptions<FacilityServiceOptions> options)
        {
            if (options.Value.FacilitiesJsonFilePath is null)
                throw new ArgumentException("В конфигурации ошибка в JsonFilePath");
            _jsonFilePath = options.Value.FacilitiesJsonFilePath;
        }

        public async Task SerializeDataJsonAsync(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks, CancellationToken token)
        {
            container = new(factories, units, tanks);

            string json = JsonSerializer.Serialize(container, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true });

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

        public string? GetFactoriesSummary()
        {
            string factorySummary = string.Empty;

            if (_factories != null)
            {
                foreach (var item in _factories)
                    factorySummary += item.ToString();
                return factorySummary;
            }
            return null;
        }

        public string? GetUnitsSummary()
        {
            if (_units != null)
            {
                string unitsSummary = string.Empty;
                foreach (var item in _units)

                    unitsSummary += item.ToString();
                return unitsSummary;
            }
            return null;
        }

        public string? GetTanksSummary()
        {
            if (_tanks != null)
            {
                string tankSummary = string.Empty;
                foreach (var item in _tanks)
                {
                    tankSummary += item.ToString();
                }
                return tankSummary;
            }
            return null;
        }

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
    }
}