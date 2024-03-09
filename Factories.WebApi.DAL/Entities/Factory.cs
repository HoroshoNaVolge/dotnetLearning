namespace Factories.WebApi.DAL.Entities
{
    public class Factory
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; set; }
    }
}