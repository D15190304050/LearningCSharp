using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
{
    namespace Sort
    {
        /// <summary>
        /// The HeapSort class provides static methods to sort an array by a binary heap.
        /// </summary>
        public class HeapSort : SortBase
        {
            /// <summary>
            /// This class should not be instantiated.
            /// </summary>
            private HeapSort() { }

            /// <summary>
            /// Sorts the objects in the specified array.
            /// </summary>
            /// <typeparam name="T">The type of object to sort, which implemets IComparable&lt;T> interface.</typeparam>
            /// <param name="array">The specified array.</param>
            public static void Sort<T>(T[] array) where T : IComparable<T>
            {
                // Create a temporary array to sort, with index 0 unused.
                int length = array.Length;
                T[] temp = new T[1 + length];
                for (int i = 0; i < length; i++)
                    temp[i + 1] = array[i];

                // Heap construction.
                for (int index = length / 2; index >= 1; index--)
                    Sink(temp, index, length);

                // Sort down.
                while (length > 1)
                {
                    Swap(temp, 1, length--);
                    Sink(temp, 1, length);
                }

                // Copy back to original array.
                for (int i = 0; i < array.Length; i++)
                    array[i] = temp[i + 1];
            }

            /// <summary>
            /// Makes the item sink to proper position.
            /// </summary>
            /// <typeparam name="T">The type of object to sort, which implemets ICompare<T> interface.</typeparam>
            /// <param name="array">The specified array to sort.</param>
            /// <param name="index">The index of object to sink.</param>
            /// <param name="size">Current size of the heap.</param>
            private static void Sink<T>(T[] array, int index, int size) where T : IComparable<T>
            {
                while (2 * index <= size)
                {
                    int nextIndex = 2 * index;
                    if (nextIndex < size && Less(array[nextIndex], array[nextIndex + 1]))
                        nextIndex++;
                    if (!Less(array[index], array[nextIndex]))
                        break;
                    Swap(array, index, nextIndex);
                    index = nextIndex;
                }
            }
        }
    }
}
