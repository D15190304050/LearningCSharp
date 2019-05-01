using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools
{
    namespace Sort
    {
        public class Quick : SortBase
        {
            private static Random random;

            static Quick()
            {
                // The random index generator, initialize only once.
                random = new Random();
            }

            /// <summary>
            /// This class should not be instantiated.
            /// </summary>
            private Quick() { }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="array"></param>
            public static void Sort<T>(T[] array) where T : IComparable<T>
            {
                // Eliminate dependence on iunput.
                Shuffle(array);
                Sort(array, 0, array.Length - 1);
            }

            private static void Sort<T>(T[] array, int low, int high) where T : IComparable<T>
            {
                if (high <= low)
                    return;

                // Partition.
                int partitioned = Partition(array, low, high);

                // Sort the left part array[low ... partitioned-1].
                Sort(array, low, partitioned - 1);

                // Sort the right part array[partitioned+1 ... high].
                Sort(array, partitioned + 1, high);
            }

            // The faster edition of quick sort.
            public static void Sort3Way<T>(T[] array) where T : IComparable<T>
            {
                // Eliminate dependence on iunput.
                Shuffle(array);
                Sort3Way(array, 0, array.Length - 1);
            }

            private static void Sort3Way<T>(T[] array, int low, int high) where T : IComparable<T>
            {
                if (high <= low)
                    return;

                int lessThan = low;
                int currentIndex = low + 1;
                int greaterThan = high;
                T pItem = array[low];

                while (currentIndex <= greaterThan)
                {
                    int cmp = array[currentIndex].CompareTo(pItem);
                    if (cmp < 0)
                        Swap(array, lessThan++, currentIndex++);
                    else if (cmp > 0)
                        Swap(array, currentIndex, greaterThan--);
                    else
                        currentIndex++;
                }

                // Now array[low ... lessThan-1] < pItem = array[lessTHan ... greaterThan] < array[greaterThan+1 ... high].
                Sort3Way(array, low, lessThan - 1);
                Sort3Way(array, greaterThan + 1, high);
            }

            /// <summary>
            /// Partition into array[low ... i-1], array[i], array[i+1 ... high].
            /// </summary>
            /// <typeparam name="T">The generic type of object to sort, which implements IComparable&lt;T> Interface.</typeparam>
            /// <param name="array">The specified array.</param>
            /// <param name="low">The index of the first accessible object in the specified array.</param>
            /// <param name="high">The index of the last accessible object in the specified array.</param>
            /// <returns>The index of the partitioned object in after partition.</returns>
            private static int Partition<T>(T[] array, int low, int high) where T : IComparable<T>
            {
                // Left and right scan indices.
                int left = low;
                int right = high + 1;

                // Partitioning item, with index low.
                T pItem = array[low];

                // The partition procedure.
                for (;;)
                {
                    while (Less(array[++left], pItem))
                    {
                        if (left == high)
                            break;
                    }
                    while (Less(pItem, array[--right]))
                    {
                        if (right == low)
                            break;
                    }
                    if (right <= left)
                        break;
                    Swap(array, left, right);
                }

                // Insert pItem into correct position with array[low ... ]
                Swap(array, low, right);
                return right;
            }

            private static void Shuffle<T>(T[] array)
            {
                for (int i = 2; i < array.Length; i++)
                {
                    int indexToSwap = random.Next() % i;
                    Swap(array, i, indexToSwap);
                }
            }
        }
    }
}
