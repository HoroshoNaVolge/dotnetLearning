using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.Other
{
    internal record class Person
    {
        /// <summary>
        /// Имя персоны
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Возраст персоны
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// Дата приёма на работу
        /// </summary>

        public string? Company { get; set; }

        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="age">Возраст</param>
        /// <param name="hireDate">Дата найма</param>
        public Person(string name, int age, DateTime hireDate)
        {
            Name = name;
            Age = age;
            HireDate = hireDate;
        }

        public Person(string name, string company)
        {
            Name = name;
            Company = company;
        }
    }
}
