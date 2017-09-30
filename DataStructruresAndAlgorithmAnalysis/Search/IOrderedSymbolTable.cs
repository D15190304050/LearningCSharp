using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
{
    namespace Search
    {
        /// <summary>
        /// An interface for symbol table which supports order based operations.
        /// This interface inherits from the ISymbolTable&lt;TKey, TValue&gt; interface.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of key in the key-value pair.
        /// The TKey here implements the &lt;IComparable&gt; interface.
        /// </typeparam>
        /// <typeparam name="TValue">The type of value in the key-value pair.</typeparam>
        interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue> where TKey : IComparable<TKey>
        {
            /// <summary>
            /// Number of keys in [low ... high].
            /// </summary>
            /// <returns></returns>
            int Size(TKey low, TKey high);

            /// <summary>
            /// Get the largest key in the ordered symbol table.
            /// Returns default value of type TKey if the ordered symbol table is empty.
            /// </summary>
            TKey MaxKey();

            /// <summary>
            /// Get the smallest key in the symbol table.
            /// Returns default value of type TKey if the ordered symbol table is empty.
            /// </summary>
            TKey MinKey();

            /// <summary>
            /// Get the largest key less than or equal to the specific key.
            /// </summary>
            TKey FloorKey(TKey key);

            /// <summary>
            /// Get the smallest key greater than or equal to the specific key.
            /// </summary>
            TKey CeilingKey(TKey key);

            /// <summary>
            /// Number of keys less than the specific key.
            /// </summary>
            int Rank(TKey key);

            /// <summary>
            /// Get the key of the specific rank;
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Thrown when (value < 0 || value > Size())
            /// </exception>
            TKey Select(int rank);

            /// <summary>
            /// Remove the key-value pair in the ordered symbol table with the largest key.
            /// This method will do nothing if the ordered symbol table is empty.
            /// </summary>
            void RemoveMax();

            /// <summary>
            /// Remove the key-value pair in the ordered symbol table with the smallest key.
            /// This method will do nothing if the ordered symbol table is empty.
            /// </summary>
            void RemoveMin();

            /// <summary>
            /// Keys in [low ... high], in sorted order.
            /// </summary>
            IEnumerable<TKey> Keys(TKey low, TKey high);
            /// <summary>
            /// Return all values in the symbol table, in sorted order (sorted by key).
            /// </summary>
            IEnumerable<TValue> Values();
        }
    }
}