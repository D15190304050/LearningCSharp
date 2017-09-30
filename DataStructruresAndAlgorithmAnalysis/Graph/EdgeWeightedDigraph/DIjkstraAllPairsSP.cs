using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedDirectedGraph
{
    /// <summary>
    /// The DijkstraAllPairsSP class represents a data type for solving the all pairs shortest paths problem in edge-weighted digraphs
    /// where the edge weights are non-negative.
    /// </summary>
    public class DijkstraAllPairsSP
    {
        // All shortest paths.
        private DijkstraShortestPaths[] all;

        /// <summary>
        /// Computes a shortest paths tree from each vertex to every other vertex in the edge-weighted digraph G.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        public DijkstraAllPairsSP(EdgeWeightedDigraph G)
        {
            all = new DijkstraShortestPaths[G.V];
            for (int v = 0; v < G.V; v++)
                all[v] = new DijkstraShortestPaths(G, v);
        }

        /// <summary>
        /// Returns a shortest path from vertex source to vertex destination as an enumerator of edges, null if nu such path.
        /// </summary>
        /// <param name="source">The source vertex of the path.</param>
        /// <param name="destination">The destination vertex of the path.</param>
        /// <returns>A shortest path from vertex source to vertex destination as an enumerator of edges, null if nu such path.</returns>
        public IEnumerable<DirectedEdge> Path(int source, int destination) { return all[source].PathTo(destination); }

        /// <summary>
        /// Returns true if there is a shortest path from vertex source to vertex destination, false otherwise.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        /// <returns>True if there is a shortest path from vertex source to vertex destination, false otherwise.</returns>
        public bool HasPath(int source, int destination) { return all[source].HasPathTo(destination); }

        /// <summary>
        /// Returns the length of a shortest path from vertex source to vertex destination, double.PositiveInifinity if no such path.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        /// <returns>the length of a shortest path from vertex source to vertex destination, double.PositiveInifinity if no such path.</returns>
        public double Distance(int source, int destination) { return all[source].DistanceTo(destination); }
    }
}