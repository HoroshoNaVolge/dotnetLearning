namespace dotnetLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tanks = GetTanks();
            var units = GetUnits();
            var factories = GetFactories();
            Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

            var foundUnit = FindUnit(units, tanks, "Резервуар 2");
            var factory = FindFactory(factories, foundUnit);

            Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

            var totalVolume = GetTotalVolume(tanks);
            Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
        }

        // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
        // можно использовать создание объектов прямо в C# коде через new, или читать из файла (на своё усмотрение)
        public static Tank[] GetTanks()
        {
            // ваш код здесь
            Tank tank1 = new(1,1500, 2000, "Резервуар 1", "Надземный-вертикальный",1);
            Tank tank2 = new(2,2500, 3000, "Резервуар 2", "Надземный-горизонтальный",1);
            Tank tank3 = new(3,3000, 3000, "Резервуар 3", "Надземный-горизонтальный",2);
            Tank tank4 = new(4,3000, 3000, "Резервуар 4", "Надземный-вертикальный",2);
            Tank tank5 = new(5,4000, 5000, "Резервуар 5", "Подземный-двустенный",2); 
            Tank tank6 = new(6,500, 500, "Резервуар 6", "Подводный",3);
            return [tank1,tank2,tank3,tank4,tank5,tank6];


        }
        // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
        public static Unit[] GetUnits()
        {
            // ваш код здесь
            Unit unit1 = new(1,"ГФУ-2", "Газофракционирующая установка", 1);
            Unit unit2 = new(2,"АВТ-6", "Атмосферно-вакуумная трубчатка", 1);
            Unit unit3 = new(3,"АВТ-10", "Атмосферно - вакуумная трубчатка",2);
            return[unit1, unit2, unit3];
        }
        // реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
        public static Factory[] GetFactories()
        {
            // ваш код здесь
            Factory factory1 = new (1,"НПЗ№1", "Первый нефтеперерабатывающий завод");
            Factory factory2 = new (2,"НПЗ№2", "Второй нефтеперерабатывающий завод");

            return [factory1, factory2];
        }

        // реализуйте этот метод, чтобы он возвращал установку (Unit), которой
        // принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
        // учтите, что по заданному имени может быть не найден резервуар
        public static Unit FindUnit(Unit[] units, Tank[] tanks, string unitName)
        {
            foreach (Tank tank in tanks)
            {
                if (tank.Name == unitName)
                {
                    foreach (Unit unit in units)
                    {
                        if (tank.UnitId == unit.Id)
                            return unit;
                    }
                }
            }
            return null;
        }

        // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
        public static Factory FindFactory(Factory[] factories, Unit unit)
        {
            foreach(Factory fact in factories)
                if(unit?.FactoryId == fact.Id)
                    return fact;
            return null;
        }

        // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
        public static int GetTotalVolume(Tank[] tanks)
        {
            var totalVolume = 0;
            foreach (Tank tank in tanks)
            {
                totalVolume += tank.Volume;
            }
            return totalVolume;
        }
    }

}

