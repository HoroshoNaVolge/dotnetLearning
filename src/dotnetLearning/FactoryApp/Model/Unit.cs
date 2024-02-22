namespace dotnetLearning.FactoryApp.Model
{
    internal class Unit(int id, string name, string description, int factoryId):IFacility
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int FactoryId { get; set; } = factoryId;
    }
}
