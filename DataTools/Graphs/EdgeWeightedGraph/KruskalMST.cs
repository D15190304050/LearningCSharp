using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedUndirectedGraph
{
    using BasicDataStructures;
    using Sort;

    /// <summary>
    /// THe KruskalMST class represents a data type for computing a minimum spanning tree in an edge-weighted graph.
    /// </summary>
    public class KruskalMST : MinSpanningTreeBase
    {
        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        public KruskalMST(EdgeWeightedGraph G)
            : base()
        {
            mst = new Queue<Edge>();

            // More efficient to build heap
            MinPriorityQueue<Edge> pq = new MinPriorityQueue<Edge>();
            foreach (Edge e in G.Edges())
                pq.Add(e);

            // Run greedy algorithm.
            UnionFind uf = new UnionFind(G.V);
            while ((!pq.IsEmpty) && (mst.Size < G.V - 1))
            {
                Edge e = pq.DeleteMin();
                int v = e.Either();
                int w = e.Other(v);

                // v-w doesn't create a cycle.
                if (!uf.Connected(v, w))
                {
                    // Merge v and w components.
                    uf.Union(v, w);
                    mst.Enqueue(e);
                    Weight += e.Weight;
                }
            }
        }
    }
}