using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
{
    namespace Search
    {
        /// <summary>
        /// A light-weight IDictionary interface.
        /// This interface doesn't support the order based operations, but OrderedSymbolTable supports.
        /// A convention here is any key can only hold one value.
        /// </summary>
        /// <typeparam name="TKey">The type of key in the key-value pair.</typeparam>
        /// <typeparam name="TValue">The type of value in the key-value pair.</typeparam>
        public interface ISymbolTable<TKey, TValue>
        {
            /// <summary>
            /// Returns Value paired with key. A new key-value pair will be added into syombol table if the key doesn't exist.
            /// </summary>
            TValue this[TKey key] { get; set; }

            /// <summary>
            /// Number of key-value pairs.
            /// </summary>
            int Size();

            /// <summary>
            /// Is this table empty?
            /// </summary>
            bool IsEmpty { get; }

            /// <summary>
            /// Add a key-value pair into the table.
            /// </summary>
            void Add(TKey key, TValue value);

            /// <summary>
            /// Add a key-value pair into the table.
            /// </summary>
            void Add(KeyValuePair<TKey, TValue> item);

            /// <summary>
            /// Empty the symbol table.
            /// </summary>
            void Clear();

            /// <summary>
            /// Return true if the symbol contains the specific key-value pair.
            /// </summary>
            bool Contains(KeyValuePair<TKey, TValue> item);

            /// <summary>
            /// Return true if there is a value paired with key.
            /// </summary>
            bool ContainsKey(TKey key);

            /// <summary>
            /// All key-value pairs in the symbol table.
            /// </summary>
            IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs();

            /// <summary>
            /// All keys in the symbol table.
            /// </summary>
            IEnumerable<TKey> Keys();

            /// <summary>
            /// Remove a specific key-value pair from the symbol table.
            /// The key-value pair will be located by the parameter item, the key-value pair itself.
            /// This method will do nothing if there isn't such a key-value pair.
            /// </summary>
            void Remove(KeyValuePair<TKey, TValue> item);

            /// <summary>
            /// Remove a specific key-value pair from the symbol table.
            /// The key-value pair will be located by the key.
            /// This method will do nothing if there isn't such a key-value pair.
            /// </summary>
            void Remove(TKey key);
        }
    }
}