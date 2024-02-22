using System.Text.Json.Serialization;

namespace dotnetLearning.FactoryApp.Model
{
    public class Factory : IFacility
    {
        public Factory() { }
        public Factory(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public override string ToString()
        {
            return $"Название завода: {Name}. Описание: {Description}. Id: {Id}.";
        }
    }
}