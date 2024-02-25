using System.Text.Json.Serialization;

namespace dotnetLearning.FactoryApp.Model
{
    public class Factory : IFacility
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; set; }

        public override string ToString() =>
            $"Название завода: {Name}. Описание: {Description}. Id: {Id}.";
    }
}