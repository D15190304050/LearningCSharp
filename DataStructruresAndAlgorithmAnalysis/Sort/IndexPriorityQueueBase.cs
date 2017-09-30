using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Sort
{
    /// <summary>
    /// The IndexPriorityQueueBase represents an indexed priority queue of generic keys.
    /// </summary>
    public abstract class IndexPriorityQueueBase<TKey> : IEnumerable<int> where TKey : IComparable<TKey>
    {
        // Max number of elements on PQ.
        protected int capacity;

        // Binary heap using 1-based indexing.
        protected int[] priorityQueue;

        // Inverse of priorityQueue.
        // priorityQueue[inversedPriorityQueue[i]] = inversedPriorityQueue[priorityQueue[i]] = i.
        protected int[] inversedPriorityQueue;

        // keys[i] = key with priority of i.
        protected TKey[] keys;

        /// <summary>
        /// Number of elements on this priority queue.
        /// </summary>
        public int Size { get; protected set; }

        /// <summary>
        /// True if this priority queue is emtpy, false otherwise.
        /// </summary>
        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        /// <summary>
        /// True if this priority queue is full, false otherwise.
        /// </summary>
        public bool IsFull
        {
            get { return Size == capacity; }
        }

        /// <summary>
        /// Initializes an empty priority queue with indecies between 0 and capacity-1.
        /// </summary>
        /// <param name="capacity">The capacity of this PQ.</param>
        protected IndexPriorityQueueBase(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("The value of capacity must be non-negative.");

            this.capacity = capacity;
            keys = new TKey[capacity + 1];
            priorityQueue = new int[capacity + 1];
            inversedPriorityQueue = new int[capacity + 1];
            for (int i = 0; i <= capacity; i++)
                inversedPriorityQueue[i] = -1;
        }

        /// <summary>
        /// Check the range of the index given from client.
        /// </summary>
        /// <param name="index">The index given from client.</param>
        protected void IndexRangeCheck(int index)
        {
            if ((index < 0) || (index >= capacity))
                throw new ArgumentOutOfRangeException(string.Format("The value of index must between [0, {0}]", capacity - 1));
        }

        /// <summary>
        /// Check whether the given index is associated with a key ot not.
        /// </summary>
        /// <param name="index">The given index.</param>
        protected void ContainsCheck(int index)
        {
            if (!Contains(index))
                throw new MemberAccessException("Index is not on priority queue.");
        }

        /// <summary>
        /// Check whether the priority queue is empty when try to access the member on it.
        /// </summary>
        protected void NotEmptyCheck()
        {
            if (Size == 0)
                throw new MemberAccessException("Priority queue underflow.");
        }

        /// <summary>
        /// Returns true if the specified index is an index on this priority queue, false otherwise.
        /// </summary>
        /// <param name="index">An index.</param>
        /// <returns>True if the specified index is an index on this priority queue, false otherwise.</returns>
        public virtual bool Contains(int index)
        {
            IndexRangeCheck(index);
            return inversedPriorityQueue[index] != -1;
        }

        /// <summary>
        /// Associate key with specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        public virtual void Add(int index, TKey key)
        {
            IndexRangeCheck(index);
            if (IsFull)
                throw new NotSupportedException("The priority queue is full.");
            if (Contains(index))
                throw new ArgumentException("Index is already in the priority queue.");

            Size++;
            priorityQueue[Size] = index;
            inversedPriorityQueue[index] = Size;
            keys[index] = key;
            Swim(Size);
        }

        /// <summary>
        /// Returns the key associated with specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The key associated with specified index.</returns>
        public TKey KeyOf(int index)
        {
            ContainsCheck(index);
            return keys[index];
        }

        /// <summary>
        /// Change the key associated with specified index to the spefied value.
        /// </summary>
        /// <param name="index">The index of the key to change.</param>
        /// <param name="key">Change the key associated with specifed index to this key.</param>
        public void ChangeKey(int index, TKey key)
        {
            IndexRangeCheck(index);
            ContainsCheck(index);

            keys[index] = key;
            Swim(inversedPriorityQueue[index]);
            Sink(inversedPriorityQueue[index]);
        }

        /// <summary>
        /// Removes the key associated with specified index.
        /// </summary>
        /// <param name="index">The index of the key to remove.</param>
        public void Delete(int index)
        {
            ContainsCheck(index);

            int i = inversedPriorityQueue[index];
            Swap(i, Size--);
            Swim(i);
            Sink(i);
            inversedPriorityQueue[index] = -1;
        }

        /// <summary>
        /// Delete the min key on the IndexMinPQ or max key on the IndexMaxPQ, and returns its associated index.
        /// </summary>
        /// <returns>The index stored in priorityQueue[1], which associated with the deleted key.</returns>
        protected int DeleteFirst()
        {
            NotEmptyCheck();

            int min = priorityQueue[1];
            Swap(1, Size--);
            Sink(1);

            // Delete.
            inversedPriorityQueue[min] = -1;

            // Not needed.
            priorityQueue[Size + 1] = -1;

            return min;
        }

        /// <summary>
        /// Decrease the key associated with specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the key to decrease.</param>
        /// <param name="key">Decrease the key associated with specified index to this key.</param>
        public virtual void DecreaseKey(int index, TKey key)
        {
            ContainsCheck(index);
            if (keys[index].CompareTo(key) <= 0)
                throw new ArgumentException("Calling DecreaseKey() with given argument would not strictly decrease the key.");
            keys[index] = key;
        }

        /// <summary>
        /// Increase the key associated with specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the key to increase.</param>
        /// <param name="key">Increase the key associated with specified index to this key.</param>
        public virtual void IncreaseKey(int index, TKey key)
        {
            ContainsCheck(index);
            if (keys[index].CompareTo(key) >= 0)
                throw new ArgumentException("Calling IncreaseKey() with given argument would not strictly increase the key.");

            keys[index] = key;
        }

        /*
        General helper funcitons.
        */

        /// <summary>
        /// A comparator which will be defined in derived class of this class according to specific situation.
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        /// <returns>True if satisfied with a specific situation, false otherwise.</returns>
        protected abstract bool Compare(int i, int j);

        /// <summary>
        /// Swap keys on this priority queue with specified indecies.
        /// </summary>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        protected void Swap(int i, int j)
        {
            int swap = priorityQueue[i];
            priorityQueue[i] = priorityQueue[j];
            priorityQueue[j] = swap;
            inversedPriorityQueue[priorityQueue[i]] = i;
            inversedPriorityQueue[priorityQueue[j]] = j;
        }

        /*
        Heap helper functions.
        */

        /// <summary>
        /// Swim the KVP to correct position in the heap.
        /// </summary>
        /// <param name="index"></param>
        protected void Swim(int index)
        {
            while ((index > 1) && (Compare(index / 2, index)))
            {
                Swap(index / 2, index);
                index /= 2;
            }
        }

        /// <summary>
        /// Sink the KVP to correct position in the heap.
        /// </summary>
        /// <param name="index"></param>
        protected void Sink(int index)
        {
            while (index * 2 <= Size)
            {
                int nextIndex = index * 2;
                if (nextIndex < Size && Compare(nextIndex, nextIndex + 1))
                    nextIndex++;
                if (!Compare(index, nextIndex))
                    break;
                Swap(index, nextIndex);
                index = nextIndex;
            }
        }

        /*
        Iterators.
        */

        /// <summary>
        /// Returns an enumrator that iterates over the keys on the priority queue,
        /// by ascending order for ImdexMinPQ and descending order for IndexMaxPQ.
        /// </summary>
        /// <returns>
        /// Returns an enumrator that iterates over the keys on the priority queue,
        /// by ascending order for ImdexMinPQ and descending order for IndexMaxPQ.
        /// </returns>
        public IEnumerator<int> GetEnumerator()
        {
            IndexMinPQ<TKey> copy = new IndexMinPQ<TKey>(priorityQueue.Length - 1);
            for (int i = 1; i <= capacity; i++)
                copy.Add(priorityQueue[i], keys[priorityQueue[i]]);

            while (!copy.IsEmpty)
                yield return copy.DeleteFirst();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}