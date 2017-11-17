using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrentProgrammingDemo
{
    public static class ConcurrentQueueDemo
    {
        /// <summary>
        /// Demonstrates: ConcurrentQueue&lt;T>.Enqueue(), ConcurrentQueue&lt;T>.TryPeek(), ConcurrentQueue&lt;T>.TryDequeue().
        /// </summary>
        public static void Demo()
        {
            // Construct a ConcurrentQueue.
            ConcurrentQueue<int> cq = new ConcurrentQueue<int>();

            // Populate the queue.
            for (int i = 0; i < 10000; i++)
                cq.Enqueue(i);

            // Peek at the first element.
            int result;
            if (!cq.TryPeek(out result))
                Console.WriteLine("CQ: TryPeek failed when it should have succeeded.");
            else if (result != 0)
                Console.WriteLine($"CQ: Expected TryPeek result of 0, got {result}.");

            int outerSum = 0;

            // Action to consume the ConcurrentQueue.
            Action action = () =>
            {
                int localSum = 0;
                int localValue;
                while (cq.TryDequeue(out localValue))
                    localSum += localValue;

                Interlocked.Add(ref outerSum, localSum);
            };

            // Start 4 concurrent consuming actions.
            Parallel.Invoke(action, action, action, action);

            Console.WriteLine($"outerSum = {outerSum}, should be 49995000.");
        }
    }
}
