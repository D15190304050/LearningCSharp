using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public static class UnitTest
    {
        private const string tinyGraph01 = @"DistributedSpanningTrees\tinyGraph01.txt";

        public static void LanStandaloneTest()
        {
            LanStandalone network = new LanStandalone(tinyGraph01);
            Console.WriteLine(network);
            network.RunDST();
            Console.WriteLine(network.GetST());
        }

        public static void LanMultiThreadTest()
        {
            LanMultiThread network = new LanMultiThread(tinyGraph01);
            Console.WriteLine(network);
            network.RunDST();
            Console.WriteLine(network.GetST());
        }
    }
}
