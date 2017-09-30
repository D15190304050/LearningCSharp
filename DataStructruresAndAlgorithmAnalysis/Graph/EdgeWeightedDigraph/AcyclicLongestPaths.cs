using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedDirectedGraph
{
    using BasicDataStructures;
    using DirectedGraph;

    /// <summary>
    /// The AcyclicLongestPaths class represents a data type for solving the single-source longest paths problem in edge-weighted directed acyclic graphs (DAGs).
    /// </summary>
    public class AcyclicLongestPaths : EdgeWeightedPathsBase
    {
        /// <summary>
        /// Computes a longest paths tree from the source vertex to every other vertex in the directed acyclic graph.
        /// </summary>
        /// <param name="G">The directed acyclic graph.</param>
        /// <param name="source">The source vertex.</param>
        public AcyclicLongestPaths(EdgeWeightedDigraph G, int source)
            : base(G, source)
        {
            // Initializes the distance info.
            for (int v = 0; v < G.V; v++)
                distanceTo[v] = double.NegativeInfinity;
            distanceTo[source] = 0.0;

            // Relax vertices in topological order.
            Topological topological = new Topological(G);
            if (!topological.HasOrder)
                throw new ArgumentException("Digraph is not acyclic.");

            foreach (int v in topological.Order)
            {
                foreach (DirectedEdge e in G.Adjacent(v))
                    Relax(e);
            }
        }

        /// <summary>
        /// Relax edge e, update if finds a "longer" path.
        /// </summary>
        /// <param name="e">The directed edge to be relaxed.</param>
        private void Relax(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();

            if (distanceTo[w] < distanceTo[v] + e.Weight)
            {
                distanceTo[w] = distanceTo[v] + e.Weight;
                edgeTo[w] = e;
            }
        }
    }
}