using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    /// <summary>
    /// The StringSortBase class provides the common static method for sorting strings for different algorithms.
    /// </summary>
    public abstract class StringSortBase
    {
        // Cut off to insertion sort.
        protected const int CutOff = 15;

        // The size of extended ASCII code.
        protected const int R = 256;

        /// <summary>
        /// Returns index=th character of s, -1 if index == length of string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="index">The index of the string.</param>
        /// <returns>index=th character of s, -1 if index == length of string.</returns>
        protected static int CharAt(string s, int index)
        {
            if ((index < 0) || (index > s.Length))
                throw new ArgumentOutOfRangeException("Index must between 0 and the length of the string.");

            if (index == s.Length)
                return -1;
            return s[index];
        }

        /// <summary>
        /// Insertion sort array[low ... high] start at index-th character.
        /// </summary>
        /// <param name="array">The string array to sort.</param>
        /// <param name="low">The minimum index to sort.</param>
        /// <param name="high">The maximum index to sort.</param>
        /// <param name="index">The index of the starting character in strings.</param>
        protected static void Insertion(string[] array, int low, int high, int index)
        {
            for (int i = low; i < high; i++)
            {
                for (int j = i; (j > low) && Less(array[j], array[j - 1], index); j--)
                    Swap(array, j, j - 1);
            }
        }

        /// <summary>
        /// Returns true if s1 less than s2 starting at character index.
        /// </summary>
        /// <param name="s1">A string.</param>
        /// <param name="s2">The other string.</param>
        /// <param name="index">The index where compare starts.</param>
        /// <returns>True if s1 less than s2 starting at character index.</returns>
        protected static bool Less(string s1, string s2, int index)
        {
            for (int i = index; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[i] < s2[i])
                    return true;
                if (s1[i] > s2[i])
                    return false;
            }
            return s1.Length < s2.Length;
        }

        /// <summary>
        /// Swap the elements in an array with index i and j respectively.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="array">The array which stores the elements to swap.</param>
        /// <param name="i">An index.</param>
        /// <param name="j">The other index.</param>
        protected static void Swap<T>(T[] array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}