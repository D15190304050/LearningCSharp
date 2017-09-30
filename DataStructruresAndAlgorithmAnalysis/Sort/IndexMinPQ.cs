using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Sort
{
    /// <summary>
    /// The IndexMinPQ class represents an indexed priority queue of generic keys.
    /// </summary>
    public class IndexMinPQ<TKey> : IndexPriorityQueueBase<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Initializes an empty index priority queue with indecies between [0,capacity-1].
        /// </summary>
        /// <param name="capacity">The keys on this priority queue are indexed from 0 to capacity-1.</param>
        public IndexMinPQ(int capacity) : base(capacity) { }

        /// <summary>
        /// Returns an index associated with the min key.
        /// </summary>
        /// <returns>An index associated with the min key.</returns>
        public int MinIndex()
        {
            NotEmptyCheck();
            return priorityQueue[1];
        }

        /// <summary>
        /// Returns a min key.
        /// </summary>
        /// <returns>A min key.</returns>
        public TKey MinKey()
        {
            NotEmptyCheck();
            return keys[priorityQueue[1]];
        }

        /// <summary>
        /// Returns a min key and returns its associated index.
        /// </summary>
        /// <returns>An index associated with the min key.</returns>
        public int DeleteMin() { return DeleteFirst(); }

        /// <summary>
        /// Decrease the key associated with specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the key to decrease.</param>
        /// <param name="key">Decrease the key associated with specified index to this key.</param>
        public override void DecreaseKey(int index, TKey key)
        {
            base.DecreaseKey(index, key);
            Swim(inversedPriorityQueue[index]);
        }

        /// <summary>
        /// Increase the key associated with specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the key to increase.</param>
        /// <param name="key">Increase the key associated with specified index to this key.</param>
        public override void IncreaseKey(int index, TKey key)
        {
            base.IncreaseKey(index, key);
            Sink(inversedPriorityQueue[index]);
        }

        /*
        General helper functions.
        */

        /// <summary>
        /// Returns true if key with index i is greater than key with index j, false otherwise.
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        /// <returns>True if key with index i is greater than key with index j, false otherwise.</returns>
        private bool Greater(int i, int j) { return keys[priorityQueue[i]].CompareTo(keys[priorityQueue[j]]) > 0; }

        /// <summary>
        /// The Compare() here is just call the Greater().
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        /// <returns>True if key with index i is greater than key with index j, false otherwise.</returns>
        protected override bool Compare(int i, int j) { return Greater(i, j); }
    }
}