using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Search
{
    public abstract class BinarySearchTreeBase<TKey, TValue> : IOrderedSymbolTable<TKey, TValue> where TKey : IComparable<TKey>
    {
        

        public abstract TValue this[TKey key] { get; set; }

        public bool IsEmpty { get { return Size() == 0; } }

        public virtual int Size() { return Size(MinKey(), MaxKey()); }
        public abstract int Size(TKey low, TKey high);
        public virtual void Add(KeyValuePair<TKey, TValue> item) { Add(item.Key, item.Value);  }
        public abstract void Add(TKey key, TValue value);
        public abstract TKey CeilingKey(TKey key);
        public abstract void Clear();
        public abstract bool Contains(KeyValuePair<TKey, TValue> item);
        public abstract bool ContainsKey(TKey key);
        public abstract TKey FloorKey(TKey key);
        public abstract IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs();
        public virtual IEnumerable<TKey> Keys() { return Keys(MinKey(), MaxKey()); }
        public abstract IEnumerable<TKey> Keys(TKey low, TKey high);
        public abstract TKey MaxKey();
        public abstract TKey MinKey();
        public abstract int Rank(TKey key);
        public abstract void Remove(TKey key);
        public abstract void Remove(KeyValuePair<TKey, TValue> item);
        public virtual void RemoveMax() { Remove(MaxKey()); }
        public virtual void RemoveMin() { Remove(MinKey()); }
        public abstract TKey Select(int rank);
        public abstract IEnumerable<TValue> Values();
    }
}
