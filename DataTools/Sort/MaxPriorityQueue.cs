﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Sort
{
    /// <summary>
    /// The MaxPriorityQueue class represenets a priority queue of generic keys.
    /// </summary>
    /// <typeparam name="TKey">The generic type of key on this priority queue.</typeparam>
    public class MaxPriorityQueue<TKey> : PriorityQueueBase<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Initializes an empty priority queue.
        /// </summary>
        public MaxPriorityQueue() : this(1) { }

        /// <summary>
        /// Initializes the priority queue with the given initial capacity.
        /// </summary>
        /// <param name="initCapacity">The initial capacity of this priority queue.</param>
        public MaxPriorityQueue(int initCapacity) : base(initCapacity) { }

        /// <summary>
        /// Initializes the priority queue from the array of keys.
        /// </summary>
        /// <param name="keys">The array of keys.</param>
        public MaxPriorityQueue(TKey[] keys) : base(keys) { }

        /// <summary>
        /// Returns true if priorityQueue[i] &lt; priorityQueue[j], false otherwise.
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        /// <returns>True if priorityQueue[i] > priorityQueue[j], false otherwise.</returns>
        protected override bool Compare(int i, int j) { return priorityQueue[i].CompareTo(priorityQueue[j]) < 0; }

        /// <summary>
        /// Removes and returns a largest key on this priority queue.
        /// </summary>
        /// <returns>A largest key on this priority queue.</returns>
        public TKey DeleteMax() => Delete();
    }
}