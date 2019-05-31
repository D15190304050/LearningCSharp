using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Search
{
    using Collections;

    // Untested.
    public class SeparateChainingHashTable<TKey, TValue> : ISymbolTable<TKey, TValue> where TKey : IComparable<TKey>
    {
        // Number of key-value pairs.
        private int size;

        // Hash table size.
        private int prime;

        // Array of ISymbolTable objects.
        private SequentialSearch<TKey, TValue>[] st;

        /// <summary>
        /// Construct a seperate chaining hash table by a default prime for hashing.
        /// </summary>
        public SeparateChainingHashTable() : this(997) { }

        /// <summary>
        /// Construct a separate chaining hash table by a specific prime for hashing.
        /// </summary>
        /// <param name="prime">The prime for hashing.</param>
        public SeparateChainingHashTable(int prime)
        {
            // Create prime linked lists.
            this.prime = prime;
            Clear();
        }

        public TValue this[TKey key]
        {
            // The get indexer seems to be more elegant than Java.
            get { return st[Hash(key)][key]; }

            set { Add(key, value); }
        }

        public bool IsEmpty
        {
            get { return size == 0; }
        }

        public void Add(KeyValuePair<TKey, TValue> item) { Add(item.Key, item.Value); }

        public void Add(TKey key, TValue value)
        {
            if (key == null || value == null)
                throw new NullReferenceException("Argument to Add() is null.");

            int index = Hash(key);
            if (!st[index].ContainsKey(key))
                size++;
            st[index].Add(key, value);
        }

        public int Capacity() { return prime; }

        public void Clear()
        {
            st = null;
            st = new SequentialSearch<TKey, TValue>[prime];
            for (int i = 0; i < prime; i++)
                st[i] = new SequentialSearch<TKey, TValue>();
            size = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
                throw new NullReferenceException("Argument to Contains() is null.");

            return st[Hash(item.Key)][item.Key].Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to ContainsKey() is null.");
            
            return st[Hash(key)].ContainsKey(key);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs()
        {
            foreach (var ss in st)
            {
                foreach (var kvp in ss.GetKeyValuePairs())
                    yield return kvp;
            }
        }

        private int Hash(TKey key) { return (key.GetHashCode() & 0x7FFFFFFF) % prime; }

        public IEnumerable<TKey> Keys()
        {
            foreach (var ss in st)
            {
                foreach (var k in ss.Keys())
                    yield return k;
            }
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to delete() is null.");

            int index = Hash(key);
            if (st[index].ContainsKey(key))
            {
                size--;
                st[index].Remove(key);
            }
        }

        public void Remove(KeyValuePair<TKey, TValue> item)
        {
            TKey key = item.Key;
            TValue value = item.Value;
            if (key == null || value == null)
                throw new NullReferenceException("Argument to Remove() is null.");

            int index = Hash(key);
            if (st[index].Contains(item))
            {
                size--;
                st[index].Remove(item);
            }
        }

        public void Resize(int prime)
        {
            this.prime = prime;
            SeparateChainingHashTable<TKey, TValue> tempSt = new SeparateChainingHashTable<TKey, TValue>(prime);
            foreach (var kvp in GetKeyValuePairs())
                tempSt.Add(kvp.Key, kvp.Value);
            this.st = tempSt.st;
        }

        public int Size()
        {
            return size;
        }
    }
}