namespace Factories.WebApi.DAL.Entities
{
    public class Tank
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; set; }
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
        public int? Volume { get; set; }
        public int? MaxVolume { get; set; }
    }
}