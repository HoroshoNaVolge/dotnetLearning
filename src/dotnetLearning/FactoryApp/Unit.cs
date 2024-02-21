namespace dotnetLearning.FactoryApp
{
    /// <summary>
    /// Установка
    /// </summary>
    internal class Unit
    {
        public int Id { get; private set; } = 1;
        public string Name { get; private set; }
        public string Description { get; private set; }

        public int FactoryId { get; private set; }

        /// <summary>
        /// Установка
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="factoryId"></param>
        public Unit(int id, string name, string description, int factoryId)
        {
            Name = name;
            Description = description;
            FactoryId = factoryId;
            Id=id;
        }


    }
}
