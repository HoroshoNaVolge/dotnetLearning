using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.Other
{
    internal static class LinqLearn
    {
        public static void Run()
        {

            /* Person2[] people =
             {
             new Person2("Tom", "Microsoft"), new Person2("Sam", "Google"),
             new Person2("Bob", "JetBrains"), new Person2("Mike", "Microsoft"),
             new Person2("Kate", "JetBrains"), new Person2("Alice", "Microsoft"),
             };

             var companies = from person in people
                             group person by person.Company;

             foreach (var company in companies)
             {
                 Console.WriteLine(company.Key);

                 foreach (var person in company)
                 {
                     Console.WriteLine(person.Name);
                 }
                 Console.WriteLine(); // для разделения между группами
            */

            Person[] people =
                  {
                      new ("Tom",44,DateTime.Parse("20.11.2021")), new ("Bob",55,DateTime.Parse("13.10.2019")),
                      new ("Rob",18,DateTime.Parse("28.01.2015")), new ("Jack",24,DateTime.Parse("11.07.2019")),
                      new ("Yennifer",18,DateTime.Parse("12.07.2019")), new ("Veronica",24, DateTime.Parse("01.01.2011")),
                  };


            var groupedPeopleLinq = from person in people
                                    group person by person.Age;

            Console.WriteLine("Используем операторы LINQ:\n");

            foreach (var dude in groupedPeopleLinq)
            {
                Console.WriteLine(dude.Key);

                foreach (var person in dude)
                {
                    Console.WriteLine(person.Name);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Используем методы расширения:\n");
            var companiesExtension = people.GroupBy(person => person.Age);

            foreach (var comp in companiesExtension)
            {
                Console.WriteLine(comp.Key);

                foreach (var person in comp)
                {
                    Console.WriteLine(person.Name);
                }
                Console.WriteLine();
            }

            // Создание нового объекта при группировке при помощи LINQ
            var pplGroupedByAgeAndCounted = from person in people
                                            group person by person.Age into g
                                            select new { Age = g.Key, Count = g.Count() }; ;

            foreach (var person in pplGroupedByAgeAndCounted)
                Console.WriteLine($"Людей с возрастом {person.Age} столько: {person.Count}");


            // Создание нового объекта при группировке при помощи LINQ, вложенный запрос имён
            var pplGroupedByAgeAndCountedExtension = 
        
                people
                .GroupBy(p => p.Age)
                .Select(g => new { Age= g.Key, Count = g.Count(), Names=g.Select(p=>p.Name) });

            foreach (var person in pplGroupedByAgeAndCountedExtension)
            {
                Console.WriteLine($"Людей с возрастом {person.Age} столько: {person.Count}");
                
                foreach(var name in person.Names)
                    Console.WriteLine(name);
            }   
        }
    }
}
