using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    /// <summary>
    /// The Arbitrage class represents provides a client that finds an arbitrage oppotunity in a currency exchange table
    /// by constructing a complete digraph representation of the exchange table and then finding a negative cycle in this digraph.
    /// </summary>
    public static class Arbitrage
    {
        /// <summary>
        /// Reads the currency excahnge table from file and prints an arbitrage oppotunity to standard output (if one exists).
        /// </summary>
        /// <param name="fullFileName">The path of the file which stores the currency exchange table.</param>
        public static void ComputeArbitrage(string fullFileName)
        {
            // Read all info from the file.
            string text = System.IO.File.ReadAllText(fullFileName);

            // Split the text into individual words.
            string[] words = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // The index of the content to be read in words[].
            // Increase by 1 whenever read content from words[].
            int currentIndex = 0;

            // V currencies.
            int V = int.Parse(words[currentIndex++]);
            string[] name = new string[V];

            // Create complete network.
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(V);
            for (int v = 0; v < V; v++)
            {
                name[v] = words[currentIndex++];
                for (int w = 0; w < V; w++)
                {
                    double rate = double.Parse(words[currentIndex++]);
                    DirectedEdge e = new DirectedEdge(v, w, -Math.Log(rate));
                    G.AddEdge(e);
                }
            }

            // Find negative cycle.
            BellmanFordShortestPaths spt = new BellmanFordShortestPaths(G, 0);
            if (spt.HasNegativeCycle)
            {
                double stake = 1000.0;
                foreach (DirectedEdge e in spt.GetNegativeCycle())
                {
                    Console.Write("{0,10:F5} {1} ", stake, name[e.From()]);
                    stake *= Math.Exp(-e.Weight);
                    Console.WriteLine("= {0,10:F5} {1}", stake, name[e.To()]);
                }
            }
            else
                Console.WriteLine("No arbitrage oppotunity.");
        }
    }
}