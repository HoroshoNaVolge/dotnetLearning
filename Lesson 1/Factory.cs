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
        public int Id { get; private set; } = 1;
        public string Name { get; private set; }    
        public string Description { get; private set; }

        public Factory(int id, string name, string description)
        {
            Name = name;
            Description = description;
            Id=id;
        }
    }
}
