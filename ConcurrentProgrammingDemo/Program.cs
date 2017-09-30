using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentProgrammingDemo
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            //CreateThreadsAndPassData.CreateAThread();
            //CreateThreadsAndPassData.PassDataToThreads();
            CreateThreadsAndPassData.RetriveDataFromThreads();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}