using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.Lesson2
{
    internal class Deal
    {
        public DateTime Date { get; set; }
        public string Id { get; set; }
        public int Sum { get; set; }
        
        public Deal(DateTime date, string id, int sum)
        {
            Date = date;
            Id = id;
            Sum = sum;
        }   
    }
}
