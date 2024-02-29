using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.FactoryApp.Model
{
    public interface IEntity<T>
    {
        // Для создания эземпляров модели при десериализации из Excel
        T Create(IDictionary<string, object?> values);
        IDictionary<string, object?> GetValues();
        IList<string> GetKeys();
    }
}
