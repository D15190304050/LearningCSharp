using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Sort
{
    /// <summary>
    /// The IndexMaxPQ class represents an indexed priority queue of generic keys.
    /// </summary>
    public class IndexMaxPQ<TKey> : IndexPriorityQueueBase<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Initializes an empty indexed priority queue with indecies between [0,capacity-1].
        /// </summary>
        /// <param name="capacity">The keys on this priority queue are indexed from 0 to capacity-1.</param>
        public IndexMaxPQ(int capacity) : base(capacity) { }

        /// <summary>
        /// Returns a max key.
        /// </summary>
        /// <returns>A max key.</returns>
        public TKey MaxKey()
        {
            NotEmptyCheck();
            return keys[priorityQueue[1]];
        }

        /// <summary>
        /// Returns an index associated with a max key.
        /// </summary>
        /// <returns>An index associated with a max key.</returns>
        public int MaxIndex()
        {
            NotEmptyCheck();
            return priorityQueue[1];
        }

        /// <summary>
        /// Removes a max key and returns its associated index.
        /// </summary>
        /// <returns>An index associated with a max key.</returns>
        public int DeleteMax() { return DeleteFirst(); }

        /// <summary>
        /// Decreases the key associated with specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the key to decrease.</param>
        /// <param name="key">Decreases the key associated with specified index to this key.</param>
        public override void DecreaseKey(int index, TKey key)
        {
            base.DecreaseKey(index, key);
            Sink(inversedPriorityQueue[index]);
        }

        /// <summary>
        /// Increases the key associated with specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the key to increase.</param>
        /// <param name="key">Increases the key associated with specified index to this key.</param>
        public override void IncreaseKey(int index, TKey key)
        {
            base.IncreaseKey(index, key);
            Swim(inversedPriorityQueue[index]);
        }

        /*
        General helper functions.
        */

        /// <summary>
        /// Returns true if key with index i is less than key with index j, false otherwise.
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        /// <returns>True if key with index i is less than key with index j, false otherwise.</returns>
        private bool Less(int i, int j) { return keys[priorityQueue[i]].CompareTo(keys[priorityQueue[j]]) < 0; }

        /// <summary>
        /// The Compare() here is just call the Less().
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        /// <returns>True if key with index i is less than key with index j, false otherwise.</returns>
        protected override bool Compare(int i, int j) { return Less(i, j); }
    }
}