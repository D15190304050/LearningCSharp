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
            string tinyGraph01 = @"DistributedSpanningTrees\tinyGraph01.txt";
            AsStandalone asStandalone = new AsStandalone(tinyGraph01);
            Console.WriteLine(asStandalone);
            asStandalone.ComputeSpanningTree();
            Console.WriteLine(asStandalone.GetSpanningTree());

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}