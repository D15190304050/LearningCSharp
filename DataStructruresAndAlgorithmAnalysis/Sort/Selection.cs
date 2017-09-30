using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
{
    namespace Sort
    {
        public class Selection : SortBase
        {
            // This class should not be instantiated.
            private Selection() { }

            // Sort array into increasing order.
            public static void Sort<T>(T[] array) where T : IComparable<T>
            {
                int length = array.Length;
                for (int i = 0; i < length; i++)
                {
                    // Exchange array[i] with smallest entry in array[i+1 ... length)
                    int indexOfMin = i;
                    for (int j = i + 1; j < length; j++)
                    {
                        if (Less(array[j], array[indexOfMin]))
                            indexOfMin = j;
                    }
                    if (indexOfMin != i)
                        Swap(array, i, indexOfMin);
                }
            }
        }
    }
}
