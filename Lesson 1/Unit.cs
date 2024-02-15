using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.Lesson1
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

        public Unit(int id, string name, string description, int factoryId)
        {
            Name = name;
            Description = description;
            FactoryId = factoryId;
            Id=id;
        }


    }
}
