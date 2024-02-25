using System.Text.Json.Serialization;

namespace dotnetLearning.FactoryApp.Model
{
    public class Unit : IFacility
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; set; }
        public int FactoryId { get; set; }

        public override string ToString() =>
            $"Название установки: {Name}. Описание: {Description}. Id: {Id}. Id завода {FactoryId}";
    }
}