using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools
{
    namespace Sort
    {
        public class Merge : SortBase
        {
            /// <summary>
            /// This class should not be instantiated.
            /// </summary>
            private Merge() { }

            /// <summary>
            /// Sorts the objects in the specified array.
            /// </summary>
            /// <typeparam name="T">The type of object to sort, which implemets IComparable&lt;T> interface.</typeparam>
            /// <param name="array">The specified array.</param>
            public static void Sort<T>(T[] array) where T : IComparable<T>
            {
                // Auxiliary array for merges.
                // Allocate space just once.
                T[] auxiliary = new T[array.Length];
                Sort(array, auxiliary, 0, array.Length - 1);
            }

            /// <summary>
            /// Sort array[low ... high].
            /// </summary>
            /// <typeparam name="T">The type of the object to sort, which implements IComparable&lt;T> interface.</typeparam>
            /// <param name="array">The specified array.</param>
            /// <param name="auxiliary">The a</param>
            /// <param name="low"></param>
            /// <param name="high"></param>
            private static void Sort<T>(T[] array, T[] auxiliary, int low, int high) where T : IComparable<T>
            {
                // Returns if there is 1 or 0 element in the array.
                if (high <= low)
                    return;

                // Get the middle index of the range to merge.
                int middle = (low + high) / 2;

                // Merge sort the left half.
                Sort(array, auxiliary, low, middle);

                // Merge sort the right half.
                Sort(array, auxiliary, middle + 1, high);

                // Merge results.
                MergeArray(array, auxiliary, low, middle, high);
            }

            // Merge array[low ... middle] with array[middle+1 ... high].
            private static void MergeArray<T>(T[] array, T[] auxiliary, int low, int middle, int high) where T : IComparable<T>
            {
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
                        array[i] = auxiliary[right++];
                    else
                        array[i] = auxiliary[left++];
                }
            }

            public static void SortBottomUp<T>(T[] array) where T : IComparable<T>
            {
                int length = array.Length;
                T[] auxiliary = new T[length];
                for (int size = 1; size < length; size += size)
                {
                    for (int low = 0; low < length - size; low += size + size)
                        MergeArray(array, auxiliary, low, low + size - 1, Min(low + size + size - 1, length - 1));
                }
            }

            private static T Min<T>(T t1, T t2) where T : IComparable<T>
            {
                return Less(t1, t2) ? t1 : t2;
            }
        }
    }
}
