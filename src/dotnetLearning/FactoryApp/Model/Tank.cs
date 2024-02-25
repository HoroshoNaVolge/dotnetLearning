using System.Text.Json.Serialization;

namespace dotnetLearning.FactoryApp.Model
{
    public class Tank : IFacility
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; set; }
        public int? UnitId { get; set; }
        public int? Volume { get; set; }
        public int? MaxVolume { get; set; }

        public override string ToString() =>
            $"Название резервуара: {Name}. Описание: {Description}. Id: {Id}. Текущий объём: {Volume}. Максимальный объём {MaxVolume}";
    }
}