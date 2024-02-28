using dotnetLearning.FactoryApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.FactoryApp.Service.ExcelSerialization
{
    public static class EntityExcelSerializationHelper
    {
        public static IDictionary<string, object?> GetValues(Factory factory) =>
                    new Dictionary<string, object?>
                    {
                { "Name", factory.Name },
                { "Description", factory.Description },
                { "Id", factory.Id },
                    };

        public static IList<string> GetFactoryKeys() => ["Name", "Description", "Id"];

        public static IDictionary<string, object?> GetValues(Unit unit) =>
            new Dictionary<string, object?>
            {
                { "Name", unit.Name },
                { "Description", unit.Description },
                { "Id", unit.Id },
                { "FactoryId", unit.FactoryId},
            };

        public static IList<string> GetUnitKeys() => ["Name", "Description", "Id", "FactoryId"];

        public static IDictionary<string, object?> GetValues(Tank tank) =>
            new Dictionary<string, object?>
            {
                { "Name", tank.Name },
                { "Description", tank.Description },
                { "Id", tank.Id },
                { "UnitId", tank.UnitId},
                { "Volume", tank.Volume},
                { "MaxVolume" ,tank.MaxVolume }
            };

        public static IList<string> GetTankKeys() => ["Name", "Description", "Id", "UnitId", "Volume", "MaxVolume"];
    }
}
