using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning
{
    /// <summary>
    /// Завод
    /// </summary>
    internal class Factory
    {
        public int Id { get; private set; }
        public string Name { get; private set; }    
        public string Description { get; private set; }

        public Factory(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
