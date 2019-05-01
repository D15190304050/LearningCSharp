using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    /// <summary>
    /// The MostSignificantDigit class provides static methods for sorting an array of extended ASCII strings or integers using most-significant digit radix sort.
    /// </summary>
    public class MostSignificantDigit : StringSortBase
    {
        /// <summary>
        /// Make this class act like a static class.
        /// </summary>
        private MostSignificantDigit() { }

        /// <summary>
        /// Sort array[low ... high], starting at the index-th character.
        /// </summary>
        /// <param name="array">The string array to sort.</param>
        /// <param name="low">The minimum index to sort.</param>
        /// <param name="high">The maximum index to sort.</param>
        /// <param name="index">The index of the starting character in strings.</param>
        /// <param name="auxiliary">The auxiliary string array.</param>
        private static void Sort(string[] array, int low, int high, int index, string[] auxiliary)
        {
            // Cut off to insertion sort for small sub-arrays.
            if (high <= low + CutOff)
            {
                Insertion(array, low, high, index);
                return;
            }

            // Compute frequency counts.
            int[] count = new int[R + 2];
            for (int i = low; i <= high; i++)
            {
                int c = CharAt(array[i], index);
                count[c + 2]++;
            }

            // Transform counts to indecies.
            for (int r = 0; r < R + 1; r++)
                count[r + 1] += count[r];

            // Distribute.
            for (int i = low; i <= high; i++)
            {
                int c = CharAt(array[i], index);
                auxiliary[count[c + 1]++] = array[i];
            }

            // Copy back.
            for (int i = low; i <= high; i++)
                array[i] = auxiliary[i - low];

            // Recursively sort for each character (excludes sentinel -1).
            for (int r = 0; r < R; r++)
                Sort(array, low + count[r], low + count[r + 1] - 1, index + 1, auxiliary);
        }

        /// <summary>
        /// Re-arranges the array of extended ASCII strings in ascending order.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        public static void Sort(string[] array)
        {
            int size = array.Length;
            string[] auxiliary = new string[size];
            Sort(array, 0, size - 1, 0, auxiliary);
        }

        /// <summary>
        /// Insertion sort array[low ... high] start at index-th character.
        /// </summary>
        /// <param name="array">The string array to sort.</param>
        /// <param name="low">The minimum index to sort.</param>
        /// <param name="high">The maximum index to sort.</param>
        /// <param name="index">The index of the starting character in strings.</param>
        private static void Insertion(int[] array, int low, int high, int index)
        {
            for (int i = low; i <= high; i++)
            {
                for (int j = i; (j > low) && (array[j] < array[j - 1]); j--)
                    Swap(array, j, j - 1);
            }
        }

        /// <summary>
        /// Most-siginificant digit sort array[low ... high], starting at the index-th byte.
        /// </summary>
        /// <param name="array">The int array to sort.</param>
        /// <param name="low">The minimum index to sort.</param>
        /// <param name="high">The maximum index to sort.</param>
        /// <param name="index">The index of the starting byte in integers.</param>
        /// <param name="auxiliary">The auxiliary int array.</param>
        private static void Sort(int[] array, int low, int high, int index, int[] auxiliary)
        {
            // Cut off to insertion sort for small sub-arrays.
            if (high <= low + CutOff)
            {
                Insertion(array, low, high, index);
                return;
            }

            // Compute frequency counts (need R = 256).
            int[] count = new int[R + 1];

            // 0xFF.
            int mask = R - 1;

            const int BitsPerInt = 32;
            const int BitsPerByte = 8;

            int shift = BitsPerInt - BitsPerByte * index - BitsPerByte;
            for (int i = low; i <= high; i++)
            {
                int c = (array[i] >> shift) & mask;
                count[c + 1]++;
            }

            // Transform counts to indecies.
            for (int r = 0; r < R; r++)
                count[r + 1] += count[r];

            // Distribute.
            for (int i = low; i <= high; i++)
            {
                int c = (array[i] >> shift) & mask;
                auxiliary[count[c]++] = array[i];
            }

            // Copy back.
            for (int i = low; i <= high; i++)
                array[i] = auxiliary[i - low];

            // No more bits.
            if (index == 4)
                return;

            // Recursively sort for each byte.
            if (count[0] > 0)
                Sort(array, low, low + count[0] - 1, index + 1, auxiliary);
            for (int r = 0; r < R; r++)
            {
                if (count[r + 1] > count[r])
                    Sort(array, low, low + count[r] - 1, index + 1, auxiliary);
            }
        }

        /// <summary>
        /// Re-arranges the array of 32-bit integers in ascending order, currently assumes that the integers are non-negative.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        public static void Sort(int[] array)
        {
            int size = array.Length;
            int[] auxiliary = new int[size];
            Sort(array, 0, size - 1, 0, auxiliary);
        }
    }
}