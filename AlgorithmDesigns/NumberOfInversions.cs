using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The NumberOfInversions class represents a static method set that provide several solutions to deal with the
    /// number of inversions problems.
    /// </summary>
    public static class NumberOfInversions
    {
        /// <summary>
        /// Solves the number of inversions problem by using merge sort.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="array">A generic array.</param>
        /// <returns>The number of inversions of the specified array.</returns>
        public static int MergeBasedCount<T>(T[] array) where T : IComparable<T>
        {
            int count = 0;
            MergeSort(array, new T[array.Length], 0, array.Length - 1, ref count);
            return count;
        }

        /// <summary>
        /// Merge sort array[low ... high] using auxiliary array.
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="array">A generic array.</param>
        /// <param name="auxiliary">A auxiliary array.</param>
        /// <param name="low">The lower limit of this merge operation.</param>
        /// <param name="high">The upper limit of this merge operation.</param>
        private static void MergeSort<T>(T[] array, T[] auxiliary, int low, int high, ref int count) where T : IComparable<T>
        {
            // Returns if there is 1 or 0 element in the array.
            if (high <= low)
                return;
            
            // Get the middle index of the range to merge.
            int middle = (low + high) / 2;

            // Merge sort the left half.
            MergeSort(array, auxiliary, low, middle, ref count);

            // Merge sort the right half.
            MergeSort(array, auxiliary, middle + 1, high, ref count);

            // Merge results and update the counter.
            count += MergeArray(array, auxiliary, low, middle, high);
        }

        /// <summary>
        /// Stably merge array[low ... middle] with array[middle+1 ... high] using auxiliary[low ... high].
        /// </summary>
        /// <typeparam name="T">T is a generic type that implements the IComparable&lt;T> interface.</typeparam>
        /// <param name="array">A generic array.</param>
        /// <param name="auxiliary">A auxiliary array.</param>
        /// <param name="low">The lower limit of this merge operation.</param>
        /// <param name="middle">The middle index of this merge operation.</param>
        /// <param name="high">The upper limit of this merge operation.</param>
        /// <param name="count">The counter that computes the number of inversions.</param>
        /// <returns>The number of inversions in sub-array[low ... high].</returns>
        private static int MergeArray<T>(T[] array, T[] auxiliary, int low, int middle, int high) where T : IComparable<T>
        {
            // Initialize counter to 0.
            int count = 0;

            // Copy original array to auxiliary array.
            for (int i = low; i <= high; i++)
                auxiliary[i] = array[i];
            
            // Scanner of sub-array[low ... middle].
            int left = low;

            // Scanner of sub-array[middle+1 ... high].
            int right = middle + 1;

            // Merge back to array.
            for (int i = low; i <= high; i++)
            {
                if (left > middle)
                    array[i] = auxiliary[right++];
                else if (right > high)
                    array[i] = auxiliary[left++];
                else if (Less(auxiliary[right], auxiliary[left]))
                {
                    count += middle - left + 1;
                    array[i] = auxiliary[right++];
                }
                else
                    array[i] = auxiliary[left++];
            }

            return count;
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