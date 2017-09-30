using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph
{
    using EdgeWeightedDirectedGraph;

    /// <summary>
    /// The topological class represents a data type for determing a topological order of a directed acyclic graph (DAG).
    /// A digrpah has a topological order if and only if it's a DAG.
    /// </summary>
    public class Topological
    {
        /// <summary>
        /// Topological order, null if no such topological order.
        /// </summary>
        public IEnumerable<int> Order { get; private set; }

        /// <summary>
        /// True if the digraph has a topological order, false otherwise.
        /// </summary>
        public bool HasOrder
        {
            get { return Order != null; }
        }

        /// <summary>
        /// rank[v] = position of vertex v in the topological order.
        /// </summary>
        private int[] rank;

        /// <summary>
        /// Determines whether digraph G has a topological order, and if so, finds such a topological order.
        /// </summary>
        /// <param name="G">The digraph.</param>
        public Topological(Digraph G)
        {
            Order = null;
            rank = new int[G.V];

            DirectedCycle finder = new DirectedCycle(G);
            if (!finder.HasCycle)
            {
                DepthFirstOrder dfs = new DepthFirstOrder(G);
                Order = dfs.ReversePostOrder;
                int r = 0;
                foreach (int v in Order)
                    rank[v] = r++;
            }
        }

        /// <summary>
        /// Determines whether the edge-weighted digraph G has a topological order, and if so, finds such a topological order.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        public Topological(EdgeWeightedDigraph G)
        {
            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(G);
            if (!finder.HasCycle)
            {
                DepthFirstOrder dfs = new DepthFirstOrder(G);
                Order = dfs.ReversePostOrder;
            }
        }

        /// <summary>
        /// Returns the position of vertex v in the topological order, -1 if the digraph is a DAG.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The position of vertex v in the topological order, -1 if the digraph is a DAG.</returns>
        public int Rank(int v)
        {
            ValidateVertex(v);
            if (HasOrder)
                return rank[v];
            else
                return -1;
        }

        /// <summary>
        /// Throw an IndexOutOfRangeException unless 0 &lt;= v &lt; V.
        /// </summary>
        /// <param name="v">The vertex.</param>
        private void ValidateVertex(int v)
        {
            int V = rank.Length;
            if ((v < 0) || (v >= V))
                throw new IndexOutOfRangeException("Vertex " + v + " is not between 0 and " + (V - 1));
        }
    }
}