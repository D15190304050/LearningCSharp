using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Sort
{
    using BasicDataStructures;

    internal static class UnitTest
    {
        /// <summary>
        /// Unit Test method for heap sort.
        /// </summary>
        public static void HeapSortUnitTest()
        {
            string[] testArray = "S O R T E X A M P L E".Split(' ');
            HeapSort.Sort(testArray);
            Show(testArray);
        }
        /* Output:
            A E E L M O P R S T X
            */

        /// <summary>
        /// Unit Test method
        /// </summary>
        public static void InsertionSortUnitTest()
        {
            string[] testArray = "S O R T E X A M P L E".Split(' ');
            Insertion.Sort(testArray);
            Show(testArray);
        }
        /* Output:
            A E E L M O P R S T X
            */

        public static void MergeSortUnitTest()
        {
            Console.WriteLine("Test for top-down merge sort:");
            string[] testArray = "M E R G E S O R T E X A M P L E".Split(' ');
            Merge.Sort(testArray);
            Show(testArray);

            Console.WriteLine("Test for bottom-up merge sort:");
            testArray = "M E R G E S O R T E X A M P L E".Split(' ');
            Merge.SortBottomUp(testArray);
            Show(testArray);
        }
        /* Output:
            Test for top-down merge sort:
            A E E E E G L M M O P R R S T X
            Test for bottom-up merge sort:
            A E E E E G L M M O P R R S T X
            */

        /// <summary>
        /// Unit test method for quick sort.
        /// </summary>
        public static void QuickSortUnitTest()
        {
            Console.WriteLine("Test for normal quick sort:");
            string[] testArray = "Q U I C K S O R T E X A M P L E".Split(' ');
            Quick.Sort(testArray);
            Show(testArray);

            Console.WriteLine("Test for 3 way quick sort:");
            testArray = "Q U I C K S O R T E X A M P L E".Split(' ');
            Quick.Sort3Way(testArray);
            Show(testArray);
        }
        /* Output:
            Test for normal quick sort:
            A C E E I K L M O P Q R S T U X
            Test for 3 way quick sort:
            A C E E I K L M O P Q R S T U X
            */

        /// <summary>
        /// Unit test method for selection sort.
        /// </summary>
        public static void SelectionSortUnitTest()
        {
            string[] testArray = "S O R T E X A M P L E".Split(' ');
            Selection.Sort(testArray);
            Show(testArray);
        }
        /* Output:
            A E E L M O P R S T X
            */

        /// <summary>
        /// Unit test method for shell sort.
        /// </summary>
        public static void ShellSortUnitTest()
        {
            string[] testArray = "S H E L L S O R T E X A M P L E".Split(' ');
            Shell.Sort(testArray);
            Show(testArray);
        }
        /* Output:
            A E E E H L L L M O P R S S T X
            */

        /// <summary>
        /// Unit test method for MaxPriorityQueue.
        /// </summary>
        public static void MaxPriorityQueueUnitTest()
        {
            Console.WriteLine("Test for priority queue:");
            string[] testArray = "S O R T E X A M P L E".Split(' ');
            MaxPriorityQueue<string> pq = new MaxPriorityQueue<string>(testArray);
            while (!pq.IsEmpty)
                Console.Write(pq.DeleteMax() + " ");
            Console.WriteLine();
        }
        /* Output:
            Test for priority queue:
            X T S R P O M L E E A
            */

        /// <summary>
        /// Unit test method for MinPriorityQueue.
        /// </summary>
        public static void MinPriorityQueueUnitTest()
        {
            string[] testArray = "S O R T E X A M P L E".Split(' ');
            MinPriorityQueue<string> pq = new MinPriorityQueue<string>(testArray);
            while (!pq.IsEmpty)
                Console.Write(pq.DeleteMin() + " ");
            Console.WriteLine();
        }

        /// <summary>
        /// Print the array in a single line.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        private static void Show<T>(T[] array)
        {
            foreach (T item in array)
                Console.Write(item.ToString() + " ");
            Console.WriteLine();
        }

        /// <summary>
        /// Unit test method for IndexMinPQ.
        /// </summary>
        public static void IndexMinPQUnitTest()
        {
            // Insert a bunch of strings.
            string[] strings = {"it", "was", "the", "best", "of", "times", "it", "was", "the", "worst" };

            IndexMinPQ<string> pq = new IndexMinPQ<string>(strings.Length);
            for (int i = 0; i < strings.Length; i++)
                pq.Add(i, strings[i]);

            // Delete and print each key.
            while (!pq.IsEmpty)
            {
                int i = pq.DeleteMin();
                Console.WriteLine(i + " " + strings[i]);
            }
            Console.WriteLine();

            // Re-insert the same strings.
            for (int i = 0; i < strings.Length; i++)
                pq.Add(i, strings[i]);

            // Print each key using the iterator.
            foreach (int i in pq)
                Console.WriteLine(i + " " + strings[i]);
        }
        /* Output:
            3 best
            0 it
            6 it
            4 of
            8 the
            2 the
            5 times
            7 was
            1 was
            9 worst

            3 best
            0 it
            6 it
            4 of
            8 the
            2 the
            5 times
            7 was
            1 was
            9 worst
            */

        /// <summary>
        /// Unit test method for IndexMaxPQ.
        /// </summary>
        public static void IndexMaxPQUnitTest()
        {
            // Insert a bunch of strings.
            string[] strings = { "it", "was", "the", "best", "of", "times", "it", "was", "the", "worst" };

            IndexMaxPQ<string> pq = new IndexMaxPQ<string>(strings.Length);
            for (int i = 0; i < strings.Length; i++)
                pq.Add(i, strings[i]);

            // Print each key using the enumerator.
            foreach (int i in pq)
                Console.WriteLine(i + " " + strings[i]);
            Console.WriteLine();

            // Increase or decrease the key.
            for (int i = 0; i < strings.Length; i++)
            {
                if (StdRandom.Uniform() < 0.5)
                    pq.IncreaseKey(i, strings[i] + strings[i]);
                else
                    pq.DecreaseKey(i, strings[i].Substring(0, 1));
            }

            // Delete and print each key.
            while (!pq.IsEmpty)
            {
                string key = pq.MaxKey();
                int i = pq.DeleteMax();
                Console.WriteLine(i + " " + key);
            }
            Console.WriteLine();

            // Re-insert the same strings.
            for (int i = 0; i < strings.Length; i++)
                pq.Add(i, strings[i]);

            // Delete them in a random order.
            int[] perm = new int[strings.Length];
            for (int i = 0; i < strings.Length; i++)
                perm[i] = i;
            StdRandom.Shuffle(perm);
            for (int i = 0; i < perm.Length; i++)
            {
                string key = pq.KeyOf(perm[i]);
                pq.Delete(perm[i]);
                Console.WriteLine(perm[i] + " " + key);
            }
        }
        /* Output:
            3 best
            6 it
            0 it
            4 of
            2 the
            8 the
            5 times
            7 was
            1 was
            9 worst

            9 worstworst
            7 w
            1 w
            5 timestimes
            8 thethe
            2 thethe
            4 o
            6 itit
            0 i
            3 b

            8 the
            6 it
            3 best
            5 times
            1 was
            7 was
            4 of
            2 the
            9 worst
            0 it
            */
    }
}