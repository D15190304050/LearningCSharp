using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    /// <summary>
    /// The CriticalPathMethod class provieds a client that solves the parallel precedence-constrained job scheduling problem via the critical path method.
    /// </summary>
    public static class CriticalPathMethod
    {
        /// <summary>
        /// Reads the precedence constrains from the file and prints a feasible schedule to standard output.
        /// </summary>
        /// <param name="fullFileName">The file which stores the jobs and its precedence.</param>
        public static void ComputeCPM(string fullFileName)
        {
            // Reads all lines from the file.
            string[] lines = System.IO.File.ReadAllLines(fullFileName);

            // Number of jobs.
            int numberOfJobs = int.Parse(lines[0]);

            // Source and sink.
            int source = numberOfJobs * 2;
            int sink = numberOfJobs * 2 + 1;

            // Build network.
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(numberOfJobs * 2 + 2);
            for (int i = 0; i < numberOfJobs; i++)
            {
                // Split the schedule info into individual words.
                string[] line = System.Text.RegularExpressions.Regex.Split(lines[i + 1], "\\s+");

                double duration = double.Parse(line[0]);
                G.AddEdge(new DirectedEdge(source, i, 0.0));
                G.AddEdge(new DirectedEdge(i + numberOfJobs, sink, 0.0));
                G.AddEdge(new DirectedEdge(i, i + numberOfJobs, duration));

                // Precedence constrains.
                for (int j = 1; j < line.Length; j++)
                {
                    int precedence = int.Parse(line[j]);
                    G.AddEdge(new DirectedEdge(i + numberOfJobs, precedence, 0.0));
                }
            }

            // Compute longest path.
            AcyclicLongestPaths lp = new AcyclicLongestPaths(G, source);

            // Print results.
            Console.WriteLine("  job  start  finish");
            Console.WriteLine("---------------------");
            for (int i = 0; i < numberOfJobs; i++)
                Console.WriteLine("{0,4} {1,7:F1} {2,7:F1}", i, lp.DistanceTo(i), lp.DistanceTo(i + numberOfJobs));
            Console.WriteLine("Finish time: {0,7:F1}", lp.DistanceTo(sink));
        }
    }
}