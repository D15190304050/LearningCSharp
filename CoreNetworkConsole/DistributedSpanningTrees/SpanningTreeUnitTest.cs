using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public static class SpanningTreeUnitTest
    {
        /// <summary>
        /// Unit test method for the AsStandlone class.
        /// </summary>
        public static void AsStandaloneTest(string graphFile)
        {
            AsStandalone asStandalone = new AsStandalone(graphFile);
            Console.WriteLine(asStandalone);
            asStandalone.ComputeSpanningTree();
            Console.WriteLine(asStandalone.GetSpanningTree());
        }

        /// <summary>
        /// Unit test method for the AsMultiThread class.
        /// </summary>
        public static void AsMultiThreadTest(string graphFile)
        {
            AsMultiThread asMultiThread = new AsMultiThread(graphFile);
            Console.WriteLine(asMultiThread);
            asMultiThread.ComputeSpanningTree();
            Console.WriteLine(asMultiThread.GetSpanningTree());
        }
    }
}
