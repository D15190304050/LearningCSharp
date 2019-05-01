using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Sort;

namespace WpfApp
{
    /// <summary>
    /// The VisualPrimMst class represents a data type for computing a minimum spanning tree in an edge-weighted graph.
    /// </summary>
    /// <remarks>
    /// Note that a minimum spanning tree forest will be computed if there number of connected components in the given edge-weighted graph is larger than 1.
    /// </remarks>
    public class VisualPrimMst
    {
        /// <summary>
        /// Edges in the MST.
        /// </summary>
        private Queue<VisualEdge> mst;

        /// <summary>
        /// Total weight of this MST.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Gets the edges in a MST (or forest) as an enumerator of VisualEdges.
        /// </summary>
        public IEnumerable<VisualEdge> Edges { get { return mst; } }

        /// <summary>
        /// marked[v] == true if v on the MST (or forest).
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// Edges with one end point on the tree.
        /// </summary>
        private MinPriorityQueue<VisualEdge> edgePQ;

        /// <summary>
        /// Computes a MST (or forest) of an VisualEdgeWeightedGraph.
        /// </summary>
        /// <param name="G">The VisualEdgeWeightedGraph.</param>
        public VisualPrimMst(VisualEdgeWeightedGraph G)
        {
            // Initialize internal data structures for processing.
            mst = new Queue<VisualEdge>();
            edgePQ = new MinPriorityQueue<VisualEdge>();
            marked = new bool[G.V];

            // Run Prim's algorithm from all vertices to get a minimum spanning tree (or forest).
            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Prim(G, v);
            }
        }

        /// <summary>
        /// Run Prim's algorithm
        /// </summary>
        /// <param name="G">The VisualEdgeWeightedGraph.</param>
        /// <param name="s">A vertex of this VisualEdgeWeightedGraph.</param>
        private void Prim(VisualEdgeWeightedGraph G, int s)
        {
            // Process the vertex s.
            Scan(G, s);

            // Better to stop when mst has (V-1) edges.
            while (!edgePQ.IsEmpty)
            {
                // Get the smallest edge on pq.
                VisualEdge e = edgePQ.DeleteMin();

                // Get 2 end-points of this edge.
                int v = e.Either();
                int w = e.Other(v);

                // lazy, bot v and w already scanned.
                if (marked[v] && marked[w])
                    continue;

                // Add e to mst.
                mst.Enqueue(e);

                // Increase the weight.
                this.Weight += e.Weight;

                // v becomes part of tree.
                if (!marked[v])
                    Scan(G, v);
                if (!marked[w])
                    Scan(G, w);
            }
        }

        /// <summary>
        /// Add all edges incident to v onto edgePQ whose other end point has not yet been scanned.
        /// </summary>
        /// <param name="G">The VisualEdgeWeightedGraph.</param>
        /// <param name="v">The next vertex to scan.</param>
        private void Scan(VisualEdgeWeightedGraph G, int v)
        {
            // Do nothing if no such vertex.
            if (G.Adjacent(v) == null)
                return;

            // Add the vertex to MST (or forest).
            marked[v] = true;

            // Add edges incident to v onto edgePQ whose other end point has not yet been scanned.
            foreach (VisualEdge e in G.Adjacent(v))
            {
                if (!marked[e.Other(v)])
                    edgePQ.Add(e);
            }
        }
    }
}
