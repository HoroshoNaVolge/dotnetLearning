using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning
{
    /// <summary>
    /// Резервуар
    /// </summary>
    internal class Tank
    {
        public int Id {  get; private set; }
        public int Volume { get; set; }
        public int MaxVolume { get; private set; }

        public string Name {  get; private set; }
        public string Description { get; private set; }

        private Unit _unit;


        public Tank(int volume, int maxVolume, string name, string description) 
        {
            Volume= volume;
            MaxVolume= maxVolume;
            Name= name;
            Description= description;
        }
    }
}
