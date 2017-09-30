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
            string filePath = @"Q:\穆雨竹\Postgraduate\Professional\CPM-01.txt";
            int source = 0;

            EdgeWeightedDigraph G = new EdgeWeightedDigraph(filePath);
            AcyclicLongestPaths lp = new AcyclicLongestPaths(G, source);
            for (int t = 0; t < G.V; t++)
            {
                if (lp.HasPathTo(t))
                {
                    Console.Write("{0} to {1} ({2:F2}) ", source, t, lp.DistanceTo(t));
                    foreach (DirectedEdge e in lp.PathTo(t))
                        Console.Write(e + "   ");
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("{0} to {1}         no path", source, t);
            }

            // Keep the console open during debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}