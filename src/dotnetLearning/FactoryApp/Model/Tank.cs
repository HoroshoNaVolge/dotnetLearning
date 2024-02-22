using System.Text.Json.Serialization;

namespace dotnetLearning.FactoryApp.Model
{
    public class Tank : IFacility
    {
        public Tank() { }
        public Tank(int id, int volume, int maxVolume, string name, string description, int unitId)
        {
            Id = id;
            Volume = volume;
            MaxVolume = maxVolume;
            Name = name;
            Description = description;
            UnitId = unitId;
        }

        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? UnitId { get; set; }
        public int? Volume { get; set; }
        public int? MaxVolume { get; set; }

        public override string ToString()
        {
            return $"Название резервуара: {Name}. Описание: {Description}. Id: {Id}. Текущий объём: {Volume}. Максимальный объём {MaxVolume}";
        }
    }
}
