using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalDataStructuresAndAlgorithm.BasicDataStructures;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The UnitTest class represents a method set of unit tests for source code in this project.
    /// </summary>
    internal static class UnitTest
    {
        /// <summary>
        /// Functionality test method for the TopK class.
        /// </summary>
        /// <param name="option">The test option for the TopK class.</param>
        /// <param name="specifiedArray">The specified array for test.</param>
        /// <param name="k">The number of elements to extract from the array.</param>
        public static void TopKFunctionalityTest(TestOption option = TestOption.DefaultIntArray, int[] specifiedArray = null, int k = 5)
        {
            // The length of the default test data.
            int length = 10;

            // Generate the data for the test.
            int[] sourceData;
            if (option == TestOption.DefaultIntArray)
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

        /// <summary>
        /// Unit test method for the TopK class.
        /// </summary>
        public static void TopKUnitTest()
        {
            // Test with default settings.
            TopKFunctionalityTest();

            // Test with specified data.
            int length = 100;
            int[] data = new int[length];
            for (int i = 0; i < data.Length; i++)
                data[i] = i;
            StdRandom.Shuffle(data);

            TopKFunctionalityTest(TestOption.SepcifiedArray, data, 10);
        }

        /// <summary>
        /// Functionality test method for the NumberOfInversions class.
        /// </summary>
        /// <param name="option">The test option for the NumberOfInversions class.</param>
        /// <param name="specifiedArray">The specified array for test.</param>
        public static void NumberOfInversionsFunctionalityTest(TestOption option = TestOption.DefaultIntArray, int[] specifiedArray = null)
        {
            // Generate the data for the test.
            int[] data;
            if (option == TestOption.DefaultIntArray)
                data = new int[] { 2, 6, 3, 4, 5, 1 };
            else
                data = specifiedArray;

            Console.WriteLine(
                "The number of inversions of the input array is {0}.", 
                NumberOfInversions.MergeBasedCount(data)
                );
        }

        /// <summary>
        /// Unit test method for the NumberOfInversions class.
        /// </summary>
        public static void NumberOfInversionsUnitTest()
        {
            // Test with default settings.
            NumberOfInversionsFunctionalityTest();

            // Test with specified data.
            int length = 10;
            int[] data = new int[length];
            for (int i = 0; i < length; i++)
                data[i] = i;
            StdRandom.Shuffle(data);

            // Print the data so that other people can examine the correctness of the program by computing the result themselves.
            foreach (int i in data)
                Console.Write(i + " ");
            Console.WriteLine();

            // Print the result.
            NumberOfInversionsFunctionalityTest(TestOption.SepcifiedArray, data);
        }
    }
}