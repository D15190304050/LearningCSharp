using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools
{
    namespace Sort
    {
        public class Insertion : SortBase
        {
            /// <summary>
            /// This class should not be instantiated.
            /// </summary>
            private Insertion() { }

            /// <summary>
            /// Sorts the objects in the specified array.
            /// </summary>
            /// <typeparam name="T">The type of object to sort, which implements IComparable&lt;T> interface.</typeparam>
            /// <param name="array">The specified array.</param>
            public static void Sort<T>(T[] array) where T : IComparable<T>
            {
                for (int i = 1; i < array.Length; i++)
                {
                    // Insert array[i] among array[i-1], array[i-2]...
                    for (int j = i; j > 0; j--)
                    {
                        if (Less(array[j], array[j - 1]))
                            Swap(array, j, j - 1);
                        else
                            break;
                    }
                }
            }
        }
    }
}
