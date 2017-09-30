using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Search
{
    public class LinearProbingHashTable<TKey, TValue> : ISymbolTable<TKey, TValue> where TKey : IComparable<TKey>
    {
        // The default capacity of linear-probing hash table.
        const int DefaultCapacity = 16;

        // Number of key-value pairs in the hash table.
        private int size;

        // Size of linear-probing hash table.
        private int capacity;

        // The keys.
        private TKey[] keys;

        // The values.
        private TValue[] values;

        public LinearProbingHashTable() : this(DefaultCapacity) { }

        public LinearProbingHashTable(int capacity)
        {
            keys = new TKey[capacity];
            values = new TValue[capacity];
            this.capacity = capacity;
        }

        private int Hash(TKey key) { return ((key.GetHashCode() & 0x7FFFFFFF) % capacity); }

        public TValue this[TKey key]
        {
            // Returns the value associated with the specific key.
            get
            {
                if (key == null)
                    throw new NullReferenceException("Argurment to indexer is null.");

                for (int index = Hash(key); (keys[index] != null) && (!keys[index].Equals(default(TKey))); index = (index + 1) % capacity)
                {
                    if (key.Equals(keys[index]))
                        return values[index];
                }

                return default(TValue);
            }

            set { Add(key, value); }
        }

        public bool IsEmpty
        {
            get { return size == 0; }
        }

        public void Add(KeyValuePair<TKey, TValue> item) { Add(item.Key, item.Value); }

        public void Add(TKey key, TValue value)
        {
            // Double capacity of current hash table to make sure the performance.
            if (size >= capacity / 2)
                Resize(2 * capacity);

            int index;
            for (index = Hash(key); ((keys[index] != null) && (!keys[index].Equals(default(TKey)))); index = (index + 1) % capacity)
            {
                if (keys[index].Equals(key))
                {
                    values[index] = value;
                    return;
                }
            }

            keys[index] = key;
            values[index] = value;
            size++;
        }

        public int Capacity() { return capacity; }

        public void Clear()
        {
            capacity = DefaultCapacity;
            keys = new TKey[capacity];
            values = new TValue[capacity];
            size = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (keys == null || values == null)
                throw new NullReferenceException("Argument to Contains() is null.");

            TKey key = item.Key;
            TValue value = item.Value;

            // An easy implementation is following:
            //return (ContainsKey(key) && (this[key].Equals(value)));
            // But that implementation will aceess keys[] twice, which is a waste of performance.

            for (int index = Hash(key); ((keys[index] != null) && (!keys[index].Equals(default(TKey)))); index = (index + 1) % capacity)
            {
                if (keys[index].Equals(key))
                    return values[index].Equals(value);
            }

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to ContainsKey() is null.");

            // The default(TValue) test is prepared for value-type test.
            // In Java, null test is enough.
            return ((this[key] != null) && (!this[key].Equals(default(TValue))));
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs()
        {
            for (int i = 0; i < capacity; i++)
            {
                if ((keys[i] != null) && (!keys[i].Equals(default(TKey))))
                    yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }

        public IEnumerable<TKey> Keys()
        {
            foreach (TKey k in keys)
            {
                if ((k != null) && (!k.Equals(default(TKey))))
                    yield return k;
            }
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to Remove() is null.");

            if (!ContainsKey(key))
                return;

            RemoveProcedure(key);
        }

        public void Remove(KeyValuePair<TKey, TValue> item)
        {
            if (keys == null)
                throw new NullReferenceException("Argument to Remove() is null.");

            if (!Contains(item))
                return;

            // Since it's a one-to-one map, the Key info is enough to delete the key-value pair.
            RemoveProcedure(item.Key);
        }

        private void RemoveProcedure(TKey key)
        {
            // Find the index of key.
            int index = Hash(key);
            while (!keys[index].Equals(key))
                index = (index + 1) % capacity;

            // Remove key and associated value.
            keys[index] = default(TKey);
            values[index] = default(TValue);

            // Re-hash all key-value pairs in the same cluster after the deleted key-value pair.
            index = (index + 1) % capacity;

            // Remove keys[index] and values[index] and re-insert them.
            while ((keys[index] != null) && (!keys[index].Equals(default(TKey))))
            {
                // Store key and value in temporary variables.
                TKey keyToReHash = keys[index];
                TValue valueToReHash = values[index];

                // Remove the key.
                keys[index] = default(TKey);
                
                // Remove the value.
                // Or even the following statement can be omitted.
                // Because the value will be overrided or associated with key null, which can't be accessed.
                values[index] = default(TValue);

                // The add method will increase the variable size by 1.
                // Decrease it here.
                size--;

                // Re-insert the key-value pair.
                Add(keyToReHash, valueToReHash);

                index = (index + 1) % capacity;
            }

            // Decrease the size, because of removing a key-value pair successfully.
            size--;

            // Halves size of array if it's 12.5% full or less.
            if (size > 0 && size < capacity / 8)
                Resize(capacity / 2);
        }

        /// <summary>
        /// Resize the hash table to the given capacity by re-hashing all of the keys.
        /// </summary>
        /// <param name="capacity"></param>
        private void Resize(int capacity)
        {
            LinearProbingHashTable<TKey, TValue> temp = new LinearProbingHashTable<TKey, TValue>(capacity);
            foreach (var kvp in GetKeyValuePairs())
                temp.Add(kvp);

            keys = temp.keys;
            values = temp.values;
            this.capacity = capacity;
        }

        public int Size() { return size; }
    }
}