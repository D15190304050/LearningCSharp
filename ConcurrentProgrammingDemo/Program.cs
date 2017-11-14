using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace ConcurrentProgrammingDemo
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            //CreateThreadsAndPassData.CreateAThread();
            //CreateThreadsAndPassData.PassDataToThreads();
            //CreateThreadsAndPassData.RetriveDataFromThreads();

            Console.WriteLine("Running the created thread.");
            Thread t = new Thread(PrintNumbers);
            t.Start();
            t.Join();

            Console.WriteLine("Running the main thread.");
            PrintNumbers();


            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }

        private static void PrintNumbers()
        {
            Console.WriteLine("Starting...");
            for (int i = 0; i < 10; i++)
                Console.WriteLine(i);
        }
    }
}