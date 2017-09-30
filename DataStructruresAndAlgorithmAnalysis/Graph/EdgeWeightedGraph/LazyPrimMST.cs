using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedUndirectedGraph
{
    using BasicDataStructures;
    using Sort;

    /// <summary>
    /// The LazyPrimMST class represents a data type for computing a MST in an edge-weighted graph.
    /// </summary>
    public class LazyPrimMST : MinSpanningTreeBase
    {
        // marked[v] = true if v on the tree.
        private bool[] marked;

        // Edges with one end-point in the tree.
        private MinPriorityQueue<Edge> pq;

        /// <summary>
        /// Compute a MST (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="G">The edge=weighted graph.</param>
        public LazyPrimMST(EdgeWeightedGraph G)
        {
            mst = new Queue<Edge>();
            pq = new MinPriorityQueue<Edge>();
            marked = new bool[G.V];

            // Run Prim from all vertices to get a min spanning forest.
            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Prim(G, v);
            }
        }

        /// <summary>
        /// Run Prim's algorithm.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="s"></param>
        private void Prim(EdgeWeightedGraph G, int s)
        {
            Scan(G, s);

            // Better to stop when mst has (V-1) edges.
            while (!pq.IsEmpty)
            {
                // Get the smallest edge on pq.
                Edge e = pq.DeleteMin();

                // Get 2 end-points of this edge.
                int v = e.Either();
                int w = e.Other(v);

                // lazy, bot v and w already scanned.
                if (marked[v] && marked[w])
                    continue;

                // Add e to mst.
                mst.Enqueue(e);

                // Increase the weight.
                Weight += e.Weight;

                // v becomes part of tree.
                if (!marked[v])
                    Scan(G, v);
                if (!marked[w])
                    Scan(G, w);
            }
        }

        /// <summary>
        /// Add all edges e incident to v onto pq the other end-point has not yet been scenned.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        /// <param name="v">The vertex.</param>
        private void Scan(EdgeWeightedGraph G, int v)
        {
            marked[v] = true;
            foreach (Edge e in G.Adjacent(v))
            {
                if (!marked[e.Other(v)])
                    pq.Add(e);
            }
        }
    }
}