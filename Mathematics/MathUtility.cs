using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    public static class MathUtility
    {
        public static T Max<T>(T[] array, out int indexOfMax) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException("The input array is null.");

            indexOfMax = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].CompareTo(array[indexOfMax]) > 0)
                    indexOfMax = i;
            }
            return array[indexOfMax];
        }

        public static T Max<T>(T[] array) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException("The input array is null.");

            int indexOfMax = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].CompareTo(array[indexOfMax]) > 0)
                    indexOfMax = i;
            }
            return array[indexOfMax];
        }

        public static T Min<T>(T[] array, out int indexOfMin) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException("The input array is null.");

            indexOfMin = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].CompareTo(array[indexOfMin]) < 0)
                    indexOfMin = i;
            }
            return array[indexOfMin];
        }

        public static T Min<T>(T[] array) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException("The input array is null.");

            int indexOfMin = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].CompareTo(array[indexOfMin]) > 0)
                    indexOfMin = i;
            }
            return array[indexOfMin];
        }
    }
}
