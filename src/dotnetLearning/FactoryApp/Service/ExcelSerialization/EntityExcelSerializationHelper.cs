using dotnetLearning.FactoryApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.FactoryApp.Service.ExcelSerialization
{
    public static class EntityExcelSerializationHelper
    {
        //static IDictionary<string, object?> GetValues(this Factory factory) => /* get values by each key for provided object */;
        //static IList<string> GetFactoryKeys() => /* get all keys for Factory */

        private static readonly Dictionary<string, Func<Factory, object>> FactoryPropertySelectors = new Dictionary<string, Func<Factory, object>>
{
    { "Id", factory => factory.Id },
    { "Name", factory => factory.Name },
    { "Description", factory => factory.Description }
};

    }
}
