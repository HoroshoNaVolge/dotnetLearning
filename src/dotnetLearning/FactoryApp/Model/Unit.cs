using System.Text.Json.Serialization;

namespace dotnetLearning.FactoryApp.Model
{
    public class Unit : IFacility
    {
        public Unit() { }

        public Unit(int id, string name, string description, int factoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            FactoryId = factoryId;
        }
        public int? Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }
        [JsonPropertyName("factoryId")]
        public int FactoryId { get; set; }

        public override string ToString()
        {
            return $"Название установки: {Name}. Описание: {Description}. Id: {Id}. Id завода {FactoryId}";
        }
    }
}
