namespace dotnetLearning.FactoryApp.Model
{
    public class Factory(int id, string name, string description):IFacility
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
    }
}