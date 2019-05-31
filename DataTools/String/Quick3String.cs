using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    using Collections;

    /// <summary>
    /// The Quick3String class provides static methods for sorting an array of strings using 3-way radix quick sort.
    /// </summary>
    public class Quick3String : StringSortBase
    {
        /// <summary>
        /// Make this class act like a static class.
        /// </summary>
        private Quick3String() { }

        /// <summary>
        /// 3-way quick sort array[low ... high] starting at index-th character.
        /// </summary>
        /// <param name="array">The string array to sort.</param>
        /// <param name="low">The minimum index to sort.</param>
        /// <param name="high">The maximum index to sort.</param>
        /// <param name="index">The index of the starting character in strings.</param>
        private static void Sort(string[] array, int low, int high, int index)
        {
            // Cut off to insertion sort for small sub-arrays.
            if (high <= low + CutOff)
            {
                Insertion(array, low, high, index);
                return;
            }

            int lessThan = low;
            int greaterThan = high;
            int v = CharAt(array[low], index);
            int i = low + 1;
            while (i <= greaterThan)
            {
                int t = CharAt(array[i], index);
                if (t < v)
                    Swap(array, lessThan++, i++);
                else if (t > v)
                    Swap(array, greaterThan--, i);
                else
                    i++;
            }

            // arrar[low ... lessThan-1] < v == array[lessThan ... greaterThan] < array[greaterThan+1 ... high].
            Sort(array, low, lessThan - 1, index);
            if (v >= 0)
                Sort(array, lessThan, greaterThan, index + 1);
            Sort(array, greaterThan + 1, high, index);
        }

        /// <summary>
        /// Re-arrange the array of strings in ascending order.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        public static void Sort(string[] array)
        {
            StdRandom.Shuffle(array);
            Sort(array, 0, array.Length - 1, 0);
        }
    }
}