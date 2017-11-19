﻿using System;
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
            const string tinyGraph01 = @"DistributedSpanningTrees\tinyGraph01.txt";
            const string tinyGraph02 = @"DistributedSpanningTrees\tinyGraph02.txt";
            const string tinyGraph03 = @"DistributedSpanningTrees\tinyGraph03.txt";

            //SpanningTreeUnitTest.AsStandaloneTest(tinyGraph02);
            SpanningTreeUnitTest.AsMultiThreadTest(tinyGraph02);

            //Thread t = new Thread(PrintNumbers);
            //t.Start();
            //t.Join();
            //Console.WriteLine(t.ThreadState);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }

        public static void PrintNumbers()
        {
            for (int i = 0; i < 10; i++)
                Console.Write(i + " ");
            Console.WriteLine();
        }
    }
}