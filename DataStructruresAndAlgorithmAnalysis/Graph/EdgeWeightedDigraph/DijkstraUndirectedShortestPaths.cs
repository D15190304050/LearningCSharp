using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedDirectedGraph
{
    using BasicDataStructures;
    using Sort;
    using EdgeWeightedUndirectedGraph;

    /// <summary>
    /// The DijkstraUndirectedShortestPaths class represents a data type for solving the single-source shortest paths problem in edge-weighted graphs
    /// where the edge weights are non-negative
    /// </summary>
    public class DijkstraUndirectedShortestPaths
    {
        // distanceTo[v] = distance of shortest source->v path.
        private double[] distanceTo;

        // edgeTo[v] = last edge on shortest source->v path.
        private Edge[] edgeTo;

        // priority queue of vertices.
        private IndexMinPQ<double> pq;

        /// <summary>
        /// Computes a shortest-paths tree from the source vertex to evety other vertex in the edge-weighted graph G.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        /// <param name="source">The source vertex.</param>
        public DijkstraUndirectedShortestPaths(EdgeWeightedGraph G, int source)
        {
            foreach (Edge e in G.Edges())
            {
                if (e.Weight < 0)
                    throw new ArgumentException("Edge " + e + " has negative weight.");
            }

            distanceTo = new double[G.V];
            edgeTo = new Edge[G.V];
            for (int v = 0; v < G.V; v++)
                distanceTo[v] = double.PositiveInfinity;
            distanceTo[source] = 0;

            // Realx edges in order of distance from s.
            pq = new IndexMinPQ<double>(G.V);
            pq.Add(source, distanceTo[source]);
            while (!pq.IsEmpty)
            {
                int v = pq.DeleteMin();
                foreach (Edge e in G.Adjacent(v))
                    Relax(e, v);
            }
        }

        /// <summary>
        /// Relax edge e and update pq if changed.
        /// </summary>
        /// <param name="e">The vertex to be relaxed.</param>
        /// <param name="v">A vertex which is already in the shortest path.</param>
        private void Relax(Edge e, int v)
        {
            int w = e.Other(v);
            if (distanceTo[w] > distanceTo[v] + e.Weight)
            {
                distanceTo[w] = distanceTo[v] + e.Weight;
                edgeTo[w] = e;
                if (pq.Contains(w))
                    pq.DecreaseKey(w, distanceTo[w]);
                else
                    pq.Add(w, distanceTo[w]);
            }
        }

        /// <summary>
        /// Returns the length of a shortest path between the source vertex and the vertex v.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>The length of a shortest path between the source vertex and the vertex v.</returns>
        public double DistanceTo(int v) { return distanceTo[v]; }

        /// <summary>
        /// Returns true if there is a path between the source vertex and the vertex v.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>True if there is a path between the source vertex and the vertex v.</returns>
        public bool HasPathTo(int v) { return distanceTo[v] < double.PositiveInfinity; }

        /// <summary>
        /// Returns a shortest path between the source vertex and vertex v.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>A shortest path between the source vertex and vertex v.</returns>
        public IEnumerable<Edge> PathTo(int v)
        {
            if (!HasPathTo(v))
                return null;

            Stack<Edge> path = new Stack<Edge>();
            int x = v;
            for (Edge e = edgeTo[v]; e != null; e = edgeTo[x])
            {
                path.Push(e);
                x = e.Other(x);
            }

            return path;
        }
    }
}