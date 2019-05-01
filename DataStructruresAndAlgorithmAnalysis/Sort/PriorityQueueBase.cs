using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Sort
{
    /// <summary>
    /// The PriorityQueueBase class represenets the base calss of a priority queue of generic keys.
    /// </summary>
    /// <typeparam name="TKey">The generic type of key on this priority queue.</typeparam>
    public abstract class PriorityQueueBase<TKey> : IEnumerable<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Store items in indeics 1 to n.
        /// </summary>
        protected TKey[] priorityQueue;

        /// <summary>
        /// Number of items in this priority queue.
        /// </summary>
        public int Size { get; protected set; }

        /// <summary>
        /// Returns true if this priority queue is empty, false otherwise.
        /// </summary>
        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        /// <summary>
        /// Initializes an empty priority queue.
        /// </summary>
        protected PriorityQueueBase() : this(1) { }

        /// <summary>
        /// Initializes an empty priority queue with the given initial capacity.
        /// </summary>
        /// <param name="initCapacity">The initial capacity of this priority queue.</param>
        protected PriorityQueueBase(int initCapacity)
        {
            priorityQueue = new TKey[1 + initCapacity];
            Size = 0;
        }

        protected PriorityQueueBase(TKey[] keys)
            : this(keys.Length)
        {
            foreach (TKey key in keys)
                Add(key);
        }

        /// <summary>
        /// Adds a new key to this priority queue.
        /// </summary>
        /// <param name="key">The key to add to this priority queue.</param>
        public void Add(TKey key)
        {
            // Double size of array if necessary.
            if (Size == priorityQueue.Length - 1)
                Resize(priorityQueue.Length * 2);

            // Add key and percolate it up to maintain heap order.
            priorityQueue[++Size] = key;
            Swim(Size);
        }

        /// <summary>
        /// Removes and returns a key on this priority queue.
        /// The key is the smallest key for min priority queue.
        /// The key is the largest key for max priority queue.
        /// </summary>
        /// <returns>A key on this priority queue with index 1.</returns>
        protected TKey Delete()
        {
            if (IsEmpty)
                throw new MemberAccessException("Priority queue underflow");

            Swap(1, Size);
            TKey root = priorityQueue[Size--];
            Sink(1);

            if ((Size > 0) && (Size == (priorityQueue.Length - 1) / 4))
                Resize(priorityQueue.Length / 2);

            return root;
        }

        /// <summary>
        /// A method for comparison between objects in priorityQueue[] with index i and j.
        /// Returns priorityQueue[i] > priorityQueue[j] for min priority queue.
        /// Returns priorityQueue[i] &lt; priorityQueue[j] for max priority queue.
        /// </summary>
        /// <param name="i">An index of the object to compare.</param>
        /// <param name="j">The other index of the object to compare.</param>
        /// <returns>True if comparison condition is satisfied, false otherwise.</returns>
        protected abstract bool Compare(int i, int j);
        
        /// <summary>
        /// Resize the priority queue array.
        /// </summary>
        /// <param name="capacity">The new capacicy.</param>
        private void Resize(int capacity)
        {
            TKey[] temp = new TKey[capacity];
            for (int i = 1; i <= Size; i++)
                temp[i] = priorityQueue[i];
            priorityQueue = temp;
        }

        /// <summary>
        /// Make the object swim to proper position.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        private void Swim(int index)
        {
            while ((index > 1) && Compare(index / 2, index))
            {
                Swap(index / 2, index);
                index /= 2;
            }
        }

        /// <summary>
        /// Make the object sink to proper position.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        private void Sink(int index)
        {
            while (2 * index <= Size)
            {
                int nextIndex = 2 * index;
                if ((nextIndex < Size) && Compare(nextIndex, nextIndex + 1))
                    nextIndex++;

                if (!Compare(index, nextIndex))
                    break;

                Swap(index, nextIndex);
                index = nextIndex;
            }
        }

        /// <summary>
        /// Swap objects in the priority queue with specified indecies.
        /// </summary>
        /// <param name="i">An index of the object to swap.</param>
        /// <param name="j">The other index of the object to swap.</param>
        protected void Swap(int i, int j)
        {
            TKey temp = priorityQueue[i];
            priorityQueue[i] = priorityQueue[j];
            priorityQueue[j] = temp;
        }

        /// <summary>
        /// Returns an enumerator to support simply iteration on this priority queue.
        /// </summary>
        /// <returns>An enumerator to support simply iteration on this priority queue.</returns>
        public IEnumerator<TKey> GetEnumerator()
        {
            for (int i = 1; i <= Size; i++)
                yield return priorityQueue[i];
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}