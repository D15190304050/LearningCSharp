using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public static class KnapsackProblem
    {
        public static double SingleItem(int totalCapacity, double[] weights, double[] values, out bool[] selectedItems)
        {
            if (totalCapacity <= 0)
                throw new ArgumentException("The capacity is negative.");
            else if ((weights == null) || (values == null))
                throw new ArgumentException("Input array is null.");
            else if (weights.Length != values.Length)
                throw new ArgumentException("Length of weights and values are not equal.");

            int numItems = values.Length;
            selectedItems = new bool[numItems];

            double[,] traces = new double[totalCapacity + 1, numItems + 1];

            for (int i = 0; i <= totalCapacity; i++)
                traces[i, 0] = 0;

            for (int item = 1; item <= numItems; item++)
            {
                for (int capacity = 0; capacity <= totalCapacity; capacity++)
                {
                    if (capacity >= weights[item - 1])
                        traces[capacity, item] = Math.Max(traces[capacity, item - 1], traces[capacity - (int)(weights[item - 1]), item - 1] + values[item - 1]);
                    else
                        traces[capacity, item] = traces[capacity, item - 1];
                }
            }

            int nextItem = numItems;
            double remainingCapacity = totalCapacity;
            while ((nextItem > 0) && (remainingCapacity > 0))
            {
                if ((traces[(int)remainingCapacity, nextItem] != traces[(int)remainingCapacity, nextItem - 1]))
                {
                    selectedItems[nextItem] = true;
                    remainingCapacity -= weights[nextItem - 1];
                }
                nextItem--;
            }

            return traces[totalCapacity, numItems];
        }
    }
}
