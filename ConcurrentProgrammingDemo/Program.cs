using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrentProgrammingDemo
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            //CreateThreadsAndPassData.CreateAThread();
            //CreateThreadsAndPassData.PassDataToThreads();
            //CreateThreadsAndPassData.RetriveDataFromThreads();

            Thread t = new Thread(PrintNumbers);
            t.Start();
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