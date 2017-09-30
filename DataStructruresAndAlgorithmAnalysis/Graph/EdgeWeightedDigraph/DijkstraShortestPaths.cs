using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedDirectedGraph
{
    using Sort;

    /// <summary>
    /// The DijkstraShortestPaths class represents a data type for solving the single source shortest paths problem in edge-weighted digraphs
    /// where the edge weights are non-negative.
    /// </summary>
    public class DijkstraShortestPaths : ShortestPathBase
    {
        // Priority queue of edges.
        private IndexMinPQ<double> pq;

        /// <summary>
        /// Computes a shortest paths tree from the vertex to every other vertex in the edge-weighted digraph G.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        /// <param name="source">The source vertex.</param>
        public DijkstraShortestPaths(EdgeWeightedDigraph G, int source)
            : base(G, source)
        {
            // Check whether there is any edge with negative weight.
            foreach (DirectedEdge e in G.Edges())
            {
                if (e.Weight < 0)
                    throw new ArgumentException("Edge " + e + " has negative weight.");
            }

            // Relax vertices in order of distance from the source vertex.
            pq = new IndexMinPQ<double>(G.V);
            pq.Add(source, distanceTo[source]);
            while (!pq.IsEmpty)
            {
                int v = pq.DeleteMin();
                foreach (DirectedEdge e in G.Adjacent(v))
                    Relax(e);
            }
        }

        /// <summary>
        /// Relax edge e and update pq if changed.
        /// </summary>
        /// <param name="e">The edge to be relaxed.</param>
        private void Relax(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();
            if (distanceTo[w] > distanceTo[v] + e.Weight)
            {
                edgeTo[w] = e;
                distanceTo[w] = distanceTo[v] + e.Weight;
                if (pq.Contains(w))
                    pq.DecreaseKey(w, distanceTo[w]);
                else
                    pq.Add(w, distanceTo[w]);
            }
        }
    }
}