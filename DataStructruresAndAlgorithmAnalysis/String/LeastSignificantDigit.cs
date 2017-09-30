using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.String
{
    /// <summary>
    /// The LeastSignificantDigit class provides static methods for sorting an array of w character strings or 32-bit integers using Least-significant digit radix sort.
    /// </summary>
    public class LeastSignificantDigit : StringSortBase
    {
        /// <summary>
        /// Make this class act like a static class.
        /// </summary>
        private LeastSignificantDigit() { }

        /// <summary>
        /// Re-arrange the array of w-character strings in ascending ordedr.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        /// <param name="length">The number of characters per string.</param>
        public static void Sort(string[] array, int length)
        {
            int size = array.Length;

            // Extend ASCII alphabet size.
            int R = 256;

            string[] auxiliary = new string[R + 1];

            // Sort by key-index counting on index-th character.
            for (int index = length - 1; index >= 0; index--)
            {
                // Compute frequency counts.
                int[] count = new int[R + 1];
                for (int i = 0; i < size; i++)
                    count[array[i][index] + 1]++;

                // Compute cumulates
                for (int r = 0; r < R; r++)
                    count[r + 1] += count[r];

                // Move data.
                for (int i = 0; i < size; i++)
                    auxiliary[count[array[i][index]]++] = array[i];

                // Copy back.
                for (int i = 0; i < size; i++)
                    array[i] = auxiliary[i];
            }
        }

        /// <summary>
        /// Re-arrange the array of 32-bit integers in ascending order.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        public static void Sort(int[] array)
        {
            // Each int is 32 bits.
            const int Bits = 32;

            const int BitsPerByte = 8;

            // Each bytes is between 0 and 255.
            const int R = 1 << BitsPerByte;

            // 0xFF.
            const int Mask = R - 1;

            // Each int is 4 bytes.
            const int length = Bits / BitsPerByte;

            int size = array.Length;
            int[] auxiliary = new int[size];

            for (int index = 0; index < length; index++)
            {
                // Compute the frequency counts.
                int[] count = new int[R + 1];
                for (int i = 0; i < size; i++)
                {
                    int temp = (array[i] >> (BitsPerByte * index)) & Mask;
                    count[temp + 1]++;
                }

                // Compute cumulates.
                for (int r = 0; r < R; r++)
                    count[r + 1] += count[r];

                // For most significant byte, 0x80-0xFF comes before 0x00-0x7F.
                if (index == length - 1)
                {
                    int shift1 = count[R] - count[R / 2];
                    int shift2 = count[R / 2];
                    for (int r = 0; r < R / 2; r++)
                        count[r] += shift1;
                    for (int r = R / 2; r < R; r++)
                        count[r] += shift2;
                }

                // Move data;
                for (int i = 0; i < size; i++)
                {
                    int temp = (array[i] >> (BitsPerByte * index)) & Mask;
                    auxiliary[count[temp]++] = array[i];
                }

                // Copy back.
                for (int i = 0; i < size; i++)
                    array[i] = auxiliary[i];
            }
        }
    }
}