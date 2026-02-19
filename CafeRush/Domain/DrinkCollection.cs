using System;
using System.Collections;

namespace CafeRush.Domain
{
    public class DrinkCollection : IEnumerable
    {
        private Drink[] items;
        private int count;

        public DrinkCollection()
        {
            items = new Drink[10];
            count = 0;
        }

        public int Count => count;

        public void Add(Drink drink)
        {
            if (count >= items.Length)
            {
                Drink[] newItems = new Drink[items.Length * 2];
                for (int i = 0; i < items.Length; i++)
                {
                    newItems[i] = items[i];
                }
                items = newItems;
            }
            items[count] = drink;
            count++;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
            {
                return;
            }
            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }
            items[count - 1] = null;
            count--;
        }

        public Drink GetAt(int index)
        {
            if (index < 0 || index >= count)
            {
                return null;
            }
            return items[index];
        }

        public void SetAt(int index, Drink drink)
        {
            if (index < 0 || index >= count)
            {
                return;
            }
            items[index] = drink;
        }

        public void Sort()
        {
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    IComparable current = items[j] as IComparable;
                    if (current != null && current.CompareTo(items[j + 1]) > 0)
                    {
                        Drink temp = items[j];
                        items[j] = items[j + 1];
                        items[j + 1] = temp;
                    }
                }
            }
        }

        public void Sort(IComparer comparer)
        {
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if (comparer.Compare(items[j], items[j + 1]) > 0)
                    {
                        Drink temp = items[j];
                        items[j] = items[j + 1];
                        items[j + 1] = temp;
                    }
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new DrinkEnumerator(this);
        }

        private class DrinkEnumerator : IEnumerator
        {
            private DrinkCollection _collection;
            private int _currentIndex;

            public DrinkEnumerator(DrinkCollection collection)
            {
                _collection = collection;
                _currentIndex = -1;
            }

            public object Current
            {
                get
                {
                    if (_currentIndex < 0 || _currentIndex >= _collection.Count)
                        return null;
                    return _collection.GetAt(_currentIndex);
                }
            }

            public bool MoveNext()
            {
                _currentIndex++;
                return _currentIndex < _collection.Count;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }
        }
    }
}