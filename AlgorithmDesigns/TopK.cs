using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalDataStructuresAndAlgorithm.Sort;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The TopK class represents a static method set that provide several solutions to deal with the top-k problems.
    /// </summary>
    public static class TopK
    {
        /// <summary>
        /// A random number generator.
        /// </summary>
        private static Random random;

        /// <summary>
        /// Initializes the random number generator.
        /// </summary>
        static TopK()
        {
            random = new Random();
        }

        /// <summary>
        /// Throws an exception if the length of the source data is not larger than k.
        /// </summary>
        /// <param name="dataLength">The length of the source data.</param>
        /// <param name="k">The number of element to extract from cadicate data.</param>
        private static void LengthCheck(int dataLength, int k)
        {
            if (dataLength < k)
                throw new ArgumentException("The length of the source data must be larger than k.");
        }

        /// <summary>
        /// Solves the top-k problem by sorting the data.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="data">The array that contains the candicate data.</param>
        /// <param name="k">The number of element to extract from cadicate data.</param>
        /// <returns>A array that contains top-k element where k is specified by the caller.</returns>
        public static T[] SortBasedExtraction<T>(T[] data, int k) where T : IComparable<T>
        {
            // Check the length before processing.
            int length = data.Length;
            LengthCheck(length, k);

            // Sort the data.
            Quick.Sort3Way(data);

            // Extract top-k elemets.
            T[] result = new T[k];
            for (int i = 0; i < k; i++)
                result[i] = data[length - 1 - i];

            return result;
        }

        /// <summary>
        /// Solves the top-k problem using a min priority queue.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="data">The array that contains the candicate data.</param>
        /// <param name="k">The number of element to extract from cadicate data.</param>
        /// <returns>A array that contains top-k element where k is specified by the caller.</returns>
        public static T[] MinPriorityQueueBasedExtraction<T>(T[] data, int k) where T : IComparable<T>
        {
            // Check the length before processing.
            int length = data.Length;
            LengthCheck(length, k);

            // Initialilze an empty min priority queue.
            MinPriorityQueue<T> queue = new MinPriorityQueue<T>();

            // Declare a variable as the loop-counter.
            int i = 0;

            // Insert (k + 1) item into the priority queue.
            while (i < k + 1)
            {
                // Add next item to the priority queue.
                queue.Add(data[i]);

                // Update the loop-counter.
                i++;
            }

            // Filter the data.
            while (i < length)
            {
                // Remove the min elemet from the priority queue.
                // Because there are (k + 1) elements in the priority queue, remove the min element will make sure that
                // the current top-k elements are still in the priority queue.
                queue.DeleteMin();

                // Add next item in the source data.
                queue.Add(data[i]);

                // Update the loop-counter.
                i++;
            }

            // For now, we have the top-(k+1) elements in the priority queue.
            // We can get the top-k elements by calling the DeleteMin() method again.
            queue.DeleteMin();

            // Extract the top-k elements from the priority queue.
            T[] result = new T[k];
            for (i = 0; i < k; i++)
                result[k - 1 - i] = queue.DeleteMin();

            return result;
        }

        /// <summary>
        /// Solves the top-k problem using a min priority queue.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="data">The array that contains the candicate data.</param>
        /// <param name="k">The number of element to extract from cadicate data.</param>
        /// <returns>A array that contains top-k element where k is specified by the caller.</returns>
        public static T[] MinPriorityQueueBasedExtraction2<T>(T[] data, int k) where T : IComparable<T>
        {
            // Check the length before processing.
            int length = data.Length;
            LengthCheck(length, k);

            // Initialize an empty min priority queue.
            MinPriorityQueue<T> queue = new MinPriorityQueue<T>();

            // Declare a variable as the loop-counter.
            int i = 0;

            // Insert k item into the priorityqueue.
            while (i < k)
            {
                // Add next item to the priority queue.
                queue.Add(data[i]);

                // Update the loop-counter.
                i++;
            }

            // Filter the data.
            while (i < length)
            {
                // Get the min element on the priority queue.
                T min = queue.Min();

                // Remove the min element if it is less than the next item in the input array.
                // And then add the item to the priority queue.
                if (min.CompareTo(data[i]) < 0)
                {
                    queue.DeleteMin();
                    queue.Add(data[i]);
                }

                // Update the loop-counter.
                i++;
            }

            // Extract the top-k elements from the priority queue.
            T[] result = new T[k];
            for (i = 0; i < k; i++)
                result[k - 1 - i] = queue.DeleteMin();

            return result;
        }

        /// <summary>
        /// Sloves the top-k problem by partitioning the data.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="data">The array that contains the candicate data.</param>
        /// <param name="k">The number of element to extract from cadicate data.</param>
        /// <returns>A array that contains top-k element where k is specified by the caller.</returns>
        public static T[] PartitionBasedExtraction<T>(T[] data, int k) where T : IComparable<T>
        {
            // Check length before processing.
            int length = data.Length;
            LengthCheck(length, k);

            // Shuffle the data before partitioning them.
            Shuffle(data);

            // Initialize the partition range.
            int low = 0;
            int high = data.Length - 1;

            // Loop until there is some element partitioned with index (data.Length - k).
            for (; ; )
            {
                // Partition some element and get its index.
                int partitionedIndex = Partition(data, low, high);

                // Update the partition range if the index of the partitioned element is not data.Length - k,
                // break otherwise.
                if (partitionedIndex > length - k)
                    high = partitionedIndex - 1;
                else if (partitionedIndex < length - k)
                    low = partitionedIndex + 1;
                else
                    break;
            }

            T[] result = new T[k];
            for (int i = 0; i < k; i++)
                result[i] = data[length - 1 - i];

            Merge.Sort(result);

            return result;
        }

        /* Helper functions for PartitionBasedExtraction(). */

        /// <summary>
        /// Partion the sub-array array[low .. high] so that array[low .. j-1] &lt;= array[j] &lt;= array[j+1 .. high]
        /// and return the index j, which is the correct index of the partitioning item.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="array">The array to partition.</param>
        /// <param name="low">The min index of the sub-array.</param>
        /// <param name="high">The max index of the sub-array.</param>
        /// <returns>The correct index of the partitioning item.</returns>
        private static int Partition<T>(T[] array, int low, int high) where T : IComparable<T>
        {
            // Left and right scan indicies.
            int left = low;
            int right = high + 1;

            // Partitioning item with index low.
            T pItem = array[low];

            // The partition procedure.
            for (; ; )
            {
                // Find item on left to swap.
                while (Less(array[++left], pItem))
                {
                    if (left == high)
                        break;
                }

                // Find item on right to swap.
                while (Less(pItem, array[--right]))
                {
                    if (low == right)
                        break;
                }

                // Check if pointers cross.
                if (right <= left)
                    break;

                Swap(array, left, right);
            }

            // Put partitioning item at array[right].
            Swap(array, low, right);

            // For now, array[low..right-1] <= array[right] <= array[right+1 .. high].
            return right;
        }

        /// <summary>
        /// Shuffles the array.
        /// </summary>
        /// <typeparam name="T">T is a generic type.</typeparam>
        /// <param name="array">The array to shuffle.</param>
        private static void Shuffle<T>(T[] array)
        {
            for (int i = 2; i < array.Length; i++)
            {
                int indexToSwap = random.Next() % i;
                Swap(array, i, indexToSwap);
            }
        }

        /// <summary>
        /// Swaps elements in the specified array with specified indicies.
        /// </summary>
        /// <typeparam name="T">T is a generic type.</typeparam>
        /// <param name="array">The array that contains the elements to swap.</param>
        /// <param name="i">The index of the element to swap.</param>
        /// <param name="j">The other index of the element to swap.</param>
        private static void Swap<T>(T[] array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        /// <summary>
        /// Returns true if x is less than y, false otherwise.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="x">A variable to compare.</param>
        /// <param name="y">The other variable to compare.</param>
        /// <returns>True if x is less than y, false otherwise.</returns>
        private static bool Less<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) < 0;
    }
}