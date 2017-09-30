using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The UnitTest class represents a method set of unit tests for source code in this project.
    /// </summary>
    internal static class UnitTest
    {
        /// <summary>
        /// Unit test method for the TopK class.
        /// </summary>
        public static void TopKUnitTest(TopKTestOption option = TopKTestOption.DefaultIntArray, int[] specifiedArray = null, int k = 5)
        {
            // The length of the default test data.
            int length = 10;

            // Generate data for the test.
            int[] sourceData;
            if (option == TopKTestOption.DefaultIntArray)
                sourceData = new int[] { 4, 5, 6, 9, 8, 7, 1, 2, 3, 0 };
            else
            {
                length = specifiedArray.Length;
                sourceData = specifiedArray;
            }


            // The array that contains the top-k elements extracted by specified method from the data.
            int[] result;

            // The array that contains the deep copy of the sourceData.
            int[] data = new int[length];

            // Test the SortBasedSelection() method.
            // Make a deep copy of the source data because the sort operation will change the array.
            Console.WriteLine("Test for the SortBasedSelection() method.");
            for (int i = 0; i < length; i++)
                data[i] = sourceData[i];
            result = TopK.SortBasedSelection(data, k);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();

            // Test the MinPriorityQueueBasedSelection() method.
            // This method will not change the original array, so there is no need for a deep copy.
            Console.WriteLine("Test for the MinPriorityQueueBasedSelection() method.");
            result = TopK.MinPriorityQueueBasedSelection(sourceData, k);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();

            // Test the PartitionBasedSelection() method.
            // Make a deep copy of the source data because the sort operation will change the array.
            Console.WriteLine("Test for the SortBasedSelection() method.");
            for (int i = 0; i < length; i++)
                data[i] = sourceData[i];
            result = TopK.PartitionBasedSelection(data, k);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();
        }
    }
}