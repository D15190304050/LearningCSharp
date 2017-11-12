using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
{
    using Graphs.EdgeWeightedDirectedGraph;

    public class Program
    {
        static int Main(string[] args)
        {
            //Graphs.UnitTest.FordFulkersonUnitTest();
            Graphs.UnitTest.ArbitrageUnitTest();

            // Keep the console open during debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}