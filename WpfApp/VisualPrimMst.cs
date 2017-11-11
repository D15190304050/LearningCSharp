using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalDataStructuresAndAlgorithm.Sort;

namespace WpfApp
{
    public class VisualPrimMst
    {
        private Queue<VisualEdge> mst;
        public double Weight { get; private set; }
        public IEnumerable<VisualEdge> Edges { get { return mst; } }

        private bool[] marked;
        private MinPriorityQueue<VisualEdge> edgePQ;

        public VisualPrimMst(VisualEdgeWeightedGraph G)
        {
            mst = new Queue<VisualEdge>();
            edgePQ = new MinPriorityQueue<VisualEdge>();
            marked = new bool[G.V];

            // Run Prim from all vertices to get a min spanning forest.
            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Prim(G, v);
            }
        }

        private void Prim(VisualEdgeWeightedGraph G, int s)
        {
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

        private void Scan(VisualEdgeWeightedGraph G, int v)
        {
            marked[v] = true;
            foreach (VisualEdge e in G.Adjacent(v))
            {
                if (!marked[e.Other(v)])
                    edgePQ.Add(e);
            }
        }
    }
}
