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
        /// The base class for sortion.
        /// This class provided the basic method for sortion process.
        /// </summary>
        public abstract class SortBase
        {
            /// <summary>
            /// Returns true if v is less than w, false otherwise.
            /// </summary>
            /// <typeparam name="T">The type of object to compare.</typeparam>
            /// <param name="v">An object to compare.</param>
            /// <param name="w">The other object to compare.</param>
            /// <returns>t=True if v is less than w, false otherwise.</returns>
            protected static bool Less<T>(T v, T w) where T : IComparable<T>
            {
                return v.CompareTo(w) < 0;
            }

            /// <summary>
            /// Swap objects in the given array with the given index i and j.
            /// </summary>
            /// <typeparam name="T">The type of object to swap.</typeparam>
            /// <param name="array">The given which stores the objects to swap.</param>
            /// <param name="i">An index of objects to swap.</param>
            /// <param name="j">The other index of object to swap.</param>
            protected static void Swap<T>(T[] array, int i, int j)
            {
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
}
