using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    using DirectedGraph;

    /// <summary>
    /// The AcyclicShortestPaths class represents a data type for solving the single-source shortest problem in edge-weighted directed acyclic digraphs (DAGs).
    /// </summary>
    public class AcyclicShortestPaths : ShortestPathBase
    {
        /// <summary>
        /// Computes a shortest paths tree from vertex source to every other vertex in the directed acyclic graph G.
        /// </summary>
        /// <param name="G">The directed acyclic graph</param>
        /// <param name="source">The source vertex.</param>
        public AcyclicShortestPaths(EdgeWeightedDigraph G, int source)
            : base(G, source)
        {
            // Visit vertices in topological order.
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
        /// Relax directed edge e, update the distance info and edgeTo[] if there is a shorter path.
        /// </summary>
        /// <param name="e">The edge to be relaxed.</param>
        private void Relax(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();
            if (distanceTo[w] > distanceTo[v] + e.Weight)
            {
                distanceTo[w] = distanceTo[v] + e.Weight;
                edgeTo[w] = e;
            }
        }
    }
}