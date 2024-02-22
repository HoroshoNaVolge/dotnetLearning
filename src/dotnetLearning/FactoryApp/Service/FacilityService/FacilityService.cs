using dotnetLearning.FactoryApp.Model;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace dotnetLearning.FactoryApp.Service.FacilityService
{
    public interface IFacilityService
    {
        public void SerializeDataJson(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks);
        public IEnumerable<IFacility> DeserializeDataJson(string filePath);
    }

    internal class FacilityService(IOptions<FacilityServiceOptions> options) : IFacilityService
    {
        void IFacilityService.SerializeDataJson(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks)
        {
            JsonContainer container = new JsonContainer(factories, units, tanks);

            var json = JsonSerializer.Serialize(container, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true });

            File.WriteAllText(options.Value.FacilitiesJsonFilePath, json);
        }

        IEnumerable<IFacility> IFacilityService.DeserializeDataJson(string filePath)
        {
            var deserializedContainer = JsonSerializer.Deserialize<JsonContainer>(filePath);

            IEnumerable<Factory> deserializedFactory = deserializedContainer.Factory;
            IEnumerable<Tank> deserializedTank = deserializedContainer.Tank;
            IEnumerable<Unit> deserializedUnit = deserializedContainer.Unit;

            IEnumerable<IFacility> result = deserializedFactory.Cast<IFacility>().Concat(deserializedUnit.Cast<IFacility>().Concat(deserializedTank.Cast<IFacility>()));
            return result;
        }
    }
}
