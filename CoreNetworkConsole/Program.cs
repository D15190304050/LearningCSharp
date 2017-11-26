using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using CoreNetworkConsole.DistributedSpanningTrees;

namespace CoreNetworkConsole
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //UnitTest.LanStandaloneTest();
            UnitTest.LanMultiThreadTest();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}