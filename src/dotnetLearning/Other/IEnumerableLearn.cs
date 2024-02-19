using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.Other
{
    internal class IEnumerableLearn : IEnumerable<int>
    {
        // Создать класс, реализующий интерфейс IEnumerable<int>. При перечислении он должен отдавать числа от 0 до 100 и обратно (то есть 0,1,2,...98,99,100,99,98,...2,1,0).
        // Класс не должен хранить в себе коллекцию этих чисел (то есть запрещено создать внутри с помощью new или Enumerable.Range список нужных чисел).
        // Возможна реализация на основе метода-итератора (yield return/yield break).
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i <= 100; i++)
                yield return i;
            for (int i = 99; i >= 0; i--)
                yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class IEnumerableLearnWithOwnEnumerator : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator() => new IEnumerableLearnOwnEnumerator();


        IEnumerator IEnumerable.GetEnumerator() =>GetEnumerator();
    }

    internal class IEnumerableLearnOwnEnumerator : IEnumerator<int>
    {
        private int current;
        private bool increasing;

        public IEnumerableLearnOwnEnumerator()
        {
            current = -1;
            increasing = true;
        }

        public int Current => current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (increasing)
            {
                current++;

                if (current > 100)
                {
                    increasing = false;
                    current-=2;
                }
            }
            else
            {
                current--;

                if (current < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            current = -1;
            increasing = true;
        }
    }
}
