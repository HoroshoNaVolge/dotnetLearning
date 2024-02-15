using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.Lesson1
{
    /// <summary>
    /// Резервуар
    /// </summary>
    internal class Tank
    {
        public int Id { get; private set; } = 1;
        public int Volume { get; set; }
        public int MaxVolume { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public int UnitId {  get; private set; }

        public Tank(int id, int volume, int maxVolume, string name, string description, int unitId)
        {
            Volume = volume;
            MaxVolume = maxVolume;
            Name = name;
            Description = description;
            UnitId = unitId;
            Id=id;
        }
    }
}
