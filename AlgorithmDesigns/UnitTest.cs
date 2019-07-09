using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Collections;

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

            // Test the SortBasedExtraction() method.
            // Make a deep copy of the source data because the sort operation will change the array.
            Console.WriteLine("Test for the SortBasedExtraction() method.");
            for (int i = 0; i < length; i++)
                data[i] = sourceData[i];
            result = TopK.SortBasedExtraction(data, k);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();

            // Test the MinPriorityQueueBasedExtraction() method.
            // This method will not change the original array, so there is no need for a deep copy.
            Console.WriteLine("Test for the MinPriorityQueueBasedExtraction() method.");
            result = TopK.MinPriorityQueueBasedExtraction(sourceData, k);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();

            // Test the MinPriorityQueueBasedExtraction2() method.
            // This method will not change the original array, so there is no need for a deep copy.
            Console.WriteLine("Test for the MinPriorityQueueBasedExtraction2() method.");
            result = TopK.MinPriorityQueueBasedExtraction2(sourceData, k);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();

            // Test the PartitionBasedExtraction() method.
            // Make a deep copy of the source data because the sort operation will change the array.
            Console.WriteLine("Test for the PartitionBasedExtraction() method.");
            for (int i = 0; i < length; i++)
                data[i] = sourceData[i];
            result = TopK.PartitionBasedExtraction(data, k);
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
            int length = 1000;
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

        public static void RoundRobinFunctionalityTest(int numPlayers)
        {
            int[][] arrangement = RoundRobin.Arrange(numPlayers);

            Console.WriteLine("We have following arrangement for the {0} players", numPlayers);
            for (int i = 0; i < arrangement.Length; i++)
            {
                for (int j = 0; j < arrangement[i].Length; j++)
                    Console.Write("{0,-4} ", arrangement[i][j]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Unit test method for DFS and BFS search of <see cref="AndOrTree" />.
        /// </summary>
        public static void AndOrTreeTest()
        {
            AndOrTree tree = new AndOrTree("TestData/Tiny AndOrTree.txt");
            Console.WriteLine("Original and or tree:");
            Console.WriteLine(tree);

            Console.WriteLine("Solution tree found by DFS without height limit:");
            Console.WriteLine(tree.DepthFirstSearch());

            Console.WriteLine("Solution tree found by DFS with height limit = 100:");
            Console.WriteLine(tree.DepthFirstSearch(100));

            Console.WriteLine("Solution tree found by BFS:");
            Console.WriteLine(tree.BreadthFirstSearch());
        }
    }
}