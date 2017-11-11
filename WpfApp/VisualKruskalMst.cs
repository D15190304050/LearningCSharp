using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalDataStructuresAndAlgorithm.BasicDataStructures;
using PersonalDataStructuresAndAlgorithm.Sort;

namespace WpfApp
{
    public class VisualKruskalMst
    {
        private System.Collections.Generic.Queue<VisualEdge> mst;
        public double Weight { get; private set; }
        public IEnumerable<VisualEdge> Edges { get { return mst; } }

        public VisualKruskalMst(VisualEdgeWeightedGraph G)
        {
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
                VisualEdge e = edgePQ.DeleteMin();
                int v = e.Either();
                int w = e.Other(v);

                // Add edge v-w to the MST if adding this edge doesn't create a cycle.
                if (!uf.Connected(v, w))
                {
                    // Merge v and w components.
                    uf.Union(v, w);
                    mst.Enqueue(e);
                    this.Weight += e.Weight;
                }
            }
        }
    }
}
