using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Graphs.DirectedGraph;
using System.IO;

namespace DataTools
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //DataTools.Collections.UnitTest.LinkedListUnitTest();
            //DataTools.Collections.UnitTest.QueueUnitTest();
            DataTools.Collections.UnitTest.StackUnitTest();

            // Keep the console open during debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}