using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools
{
    namespace Sort
    {
        public class Shell : SortBase
        {
            // This class should not be instantiated.
            private Shell() { }

            public static void Sort<T>(T[] array) where T : IComparable<T>
            {
                // Initialize the h sequence.
                int length = array.Length;
                int h = 1;
                while (h < length / 3)
                    h = h * 3 + 1;

                // Shell sort.
                while (h >= 1)
                {
                    // h sort the array.
                    for (int i = h; i < length; i++)
                    {
                        // Insert array[i] among a[i-h], a[i-2h]...
                        for (int j = i; j >= h; j -= h)
                        {
                            if (Less(array[j], array[j - h]))
                                Swap(array, j, j - h);
                        }
                    }
                    h /= 3;
                }
            }
        }
    }
}
