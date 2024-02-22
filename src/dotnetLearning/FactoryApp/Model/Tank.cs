namespace dotnetLearning.FactoryApp.Model
{
    internal class Tank(int id, int volume, int maxVolume, string name, string description, int unitId):IFacility
    {
        public int Id { get; set; } = id;
        public int Volume { get; set; } = volume;
        public int MaxVolume { get; set; } = maxVolume;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int UnitId { get; set; } = unitId;
    }
}
