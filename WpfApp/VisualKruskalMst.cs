using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.BasicDataStructures;
using DataTools.Sort;

namespace WpfApp
{
    /// <summary>
    /// The VisualKruskalMst class represents a data type for computing a minimum spanning tree in an edge-weighted graph.
    /// </summary>
    /// <remarks>
    /// Note that a minimum spanning tree forest will be computed if there number of connected components in the given edge-weighted graph is larger than 1.
    /// </remarks>
    public class VisualKruskalMst
    {
        /// <summary>
        /// Edges in the MST.
        /// </summary>
        private System.Collections.Generic.Queue<VisualEdge> mst;

        /// <summary>
        /// Total weight of this MST.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Gets the edges in a MST (or forest) as an enumerator of VisualEdges.
        /// </summary>
        public IEnumerable<VisualEdge> Edges { get { return mst; } }

        /// <summary>
        /// Computes a minimum spanning tree (or forest) of a VisualEdgeWeightedGraph.
        /// </summary>
        /// <param name="G">The VisualEdgeWeightedGraph.</param>
        public VisualKruskalMst(VisualEdgeWeightedGraph G)
        {
            // Initialize the data structure for computation.
            this.Weight = 0;
            mst = new System.Collections.Generic.Queue<VisualEdge>();

            // More efficient to build heap.
            MinPriorityQueue<VisualEdge> edgePQ = new MinPriorityQueue<VisualEdge>();
            foreach (VisualEdge e in G.Edges())
                edgePQ.Add(e);

            // Run greedy algorithm.
            UnionFind uf = new UnionFind(G.V);
            while ((!edgePQ.IsEmpty) && (mst.Count < G.V - 1))
            {
                // Get the next edge with minimum weight.
                VisualEdge e = edgePQ.DeleteMin();

                // Get 2 end points of this edge.
                int v = e.Either();
                int w = e.Other(v);

                // Add edge v-w to the MST if adding this edge doesn't create a cycle.
                if (!uf.Connected(v, w))
                {
                    // Merge v and w components.
                    uf.Union(v, w);

                    // Add this edge to MST and update the total weight.
                    mst.Enqueue(e);
                    this.Weight += e.Weight;
                }
            }
        }
    }
}
