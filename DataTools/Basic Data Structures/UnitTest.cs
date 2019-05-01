using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.BasicDataStructures
{
    /// <summary>
    /// The UnitTest class provides static methods to unit test classes provided in
    /// DataTools.BasicDataStructures namespace.
    /// </summary>
    internal static class UnitTest
    {
        /// <summary>
        /// Unit test method for LinkedList.
        /// </summary>
        public static void LinkedListUnitTest()
        {
            // Create and initialize the linked list.
            LinkedList<int> intList = new LinkedList<int>();
            for (int i = 0; i < 5; i++)
                intList.AddLast(i);
            for (int i = 5; i < 10; i++)
                intList.AddFirst(i);
            // Then the linked list hold the int sequence [9, 8, 7, 6, 5, 0, 1, 2, 3, 4]

            // Output the int sequence from head to end.
            Console.WriteLine("The int sequence from head to end:");
            foreach (int i in intList)
                Console.Write(i + " ");
            Console.WriteLine();

            // Delete the first integer from the linked list and then output the int sequence from end to head.
            int first = intList.RemoveFirst();
            Console.WriteLine("The first integer delete from the linked list is: {0}", first);
            Console.WriteLine("The int sequence from end to head:");
            foreach (int i in intList.Reverse())
                Console.Write(i + " ");
            Console.WriteLine();

            // Delete the last integer from the linked list and then output the int sequence from end to head.
            int last = intList.RemoveLast();
            Console.WriteLine("The last integer delete from the linked list is: {0}", last);
            Console.WriteLine("The int sequence from end to head:");
            foreach (int i in intList.Reverse())
                Console.Write(i + " ");
            Console.WriteLine();

            // Output the current size of the linked list.
            Console.WriteLine("Current size of the linked list is: {0}", intList.Size);

            // Get a number from the linked list by the indexer.
            Console.WriteLine("Element in the linked list with index 3 is: {0}", intList[3]);
        }
        /* Output:
            The int sequence from head to end:
            9 8 7 6 5 0 1 2 3 4
            The first integer delete from the linked list is: 9
            The int sequence from end to head:
            4 3 2 1 0 5 6 7 8
            The last integer delete from the linked list is: 4
            The int sequence from end to head:
            3 2 1 0 5 6 7 8
            Current size of the linked list is: 8
            Element in the linked list with index 3 is: 5
            */

        /// <summary>
        /// Unit test method for Queue.
        /// </summary>
        public static void QueueUnitTest()
        {
            // Create and initialize the queue.
            Queue<int> intQueue = new Queue<int>();
            for (int i = 0; i < 5; i++)
                intQueue.Enqueue(i);
            /* Current content: 0, 1, 2, 3, 4] */

            // Get the size
            int size = intQueue.Size;
            Console.WriteLine("The size of the queue is: {0}", size);

            // Output the content by queue order.
            Console.WriteLine("Content inside the queue is: ");
            foreach (int i in intQueue)
                Console.Write(i + " ");
            Console.WriteLine();

            // Delete the first integer from the queue.
            // Then output the size and the rest content.
            int first = intQueue.Dequeue();
            Console.WriteLine("The first integer in the original queue is: {0}", first);
            Console.WriteLine("The size of the cuurent queue is: {0}", intQueue.Size);
            foreach (int i in intQueue)
                Console.Write(i + " ");
            Console.WriteLine();
        }
        /* Output:
            The size of the queue is: 5
            Content inside the queue is:
            0 1 2 3 4
            The first integer in the original queue is: 0
            The size of the cuurent queue is: 4
            1 2 3 4
            */

        /// <summary>
        /// Unit test method for Stack.
        /// </summary>
        public static void StackUnitTest()
        {
            // Create and initialize the stack.
            BasicDataStructures.Stack<int> intStack = new Stack<int>();
            for (int i = 0; i < 5; i++)
                intStack.Push(i);
            /* Current content: [4, 3, 2, 1, 0] */

            // Get the size and output the integers.
            Console.WriteLine("Current size of the stack is: {0}", intStack.Size);
            Console.WriteLine("Current content in the stack is:");
            foreach (int i in intStack)
                Console.Write(i + " ");
            Console.WriteLine();

            // Pop an integer from the stack and show the size and contents.
            int top = intStack.Pop();
            Console.WriteLine("The original top integer in the stack is: {0}", top);
            Console.WriteLine("Current size of the stack is: {0}", intStack.Size);
            Console.WriteLine("Current content in the stack is:");
            foreach (int i in intStack)
                Console.Write(i + " ");
            Console.WriteLine();
        }
        /* Output:
            Current size of the stack is: 5
            Current content in the stack is:
            4 3 2 1 0
            The original top integer in the stack is: 4
            Current size of the stack is: 4
            Current content in the stack is:
            3 2 1 0
            */

        /// <summary>
        /// Unit test method for UnionFind.
        /// </summary>
        public static void UnionFindUnitTest()
        {
            string path = @"Q:\穆雨竹\Computer Science\C#\Source Codes\Data Structure and Algorithm Analysis\Test Data";
            string fileName = "tinyUF.txt";

            // Read number of sites and initialize them.
            string[] content = System.IO.File.ReadAllLines(path + @"\" + fileName);
            int sites = Convert.ToInt32(content[0]);

            UnionFind uf = new UnionFind(sites);
            for (int i = 1; i < content.Length; i++)
            {
                // Read pair to connect
                string[] testSites = content[i].Split(' ');
                int p = Convert.ToInt32(testSites[0]);
                int q = Convert.ToInt32(testSites[1]);

                // Ignore if connected.
                if (uf.Connected(p, q))
                    continue;
                // Combine components and print connection.
                uf.Union(p, q);
                Console.WriteLine(p + " " + q);
            }
            Console.WriteLine(uf.ComponentCount + " components");
        }
        /* Output:
            4 3
            3 8
            6 5
            9 4
            2 1
            5 0
            7 2
            6 1
            2 components
            */

        /// <summary>
        /// Unit test method for StdRandom.
        /// </summary>
        public static void StdRandomUnitTest()
        {
            int length = 5;
            double[] probabilities = { 0.5, 0.3, 0.1, 0.1 };
            int[] frequencies = { 5, 3, 1, 1 };
            string[] array = "A B C D E F G".Split(' ');

            for (int i = 0; i < length; i++)
            {
                Console.Write("{0,2} ", StdRandom.Uniform(100));
                Console.Write("{0:f6} ", StdRandom.Uniform(10.0, 99.0));
                Console.Write("{0,-6} ", StdRandom.Bernouli());
                Console.Write("{0:f6} ", StdRandom.Gaussian(9.0, 0.2));
                Console.Write("{0,2} ", StdRandom.Discrete(probabilities));
                Console.Write("{0,2}", StdRandom.Discrete(frequencies));
                Console.WriteLine();
            }
        }
        /* Output:
            70 59.550826 True   8.709509  2  1
             3 48.720875 False  8.792121  0  0
            54 60.061632 False  9.132748  0  0
            40 60.473302 False  8.233521  0  0
            93 52.835252 True   9.219076  0  0
            */
    }
}