using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
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
                if (high <= low)
                    return;

                int middle = low + (high - low) / 2;
                Sort(array, auxiliary, low, middle);                // Sort the left half.
                Sort(array, auxiliary, middle + 1, high);           // Sort the right half.
                MergeArray(array, auxiliary, low, middle, high);    // Merge results.
            }

            // Merge array[low ... middle] with array[middle+1 ... high].
            private static void MergeArray<T>(T[] array, T[] auxiliary, int low, int middle, int high) where T : IComparable<T>
            {
                // Copy original array to auxiliary array.
                for (int i = low; i <= high; i++)
                    auxiliary[i] = array[i];

                // Merge back to array.
                int left = low;     // The index of items in sub-array[low ... middle].
                int right = middle + 1;   // The index of items in sub-array[middle+1 ... high].
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
