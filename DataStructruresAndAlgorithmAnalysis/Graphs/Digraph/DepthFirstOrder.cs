using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph
{
    using BasicDataStructures;
    using EdgeWeightedDirectedGraph;

    /// <summary>
    /// The DepthFirstOrder class represents a data type for determing depth-first order search ordering of the vertices in a digraph or edge-weighted digaph,
    /// including pre-order, post-order and reverse post-order.
    /// </summary>
    public class DepthFirstOrder
    {
        // marked[v] = true if v has been marked in DFS.
        private bool[] marked;

        // Pre-order number of v.
        private int[] preOrderNumber;

        // Post-order number of v.
        private int[] postOrderNumber;

        // Vertices in pre-order.
        private Queue<int> preOrder;

        // Vertices in post-order.
        private Queue<int> postOrder;

        // Counter for pre-order numbering.
        private int preCounter;

        // Counter for post-order numbering.
        private int postCounter;

        private Stack<int> reversePostOrder;

        /// <summary>
        /// The number of vertices in pre-order.
        /// </summary>
        public IEnumerable<int> PreOrder
        {
            get { return preOrder; }
        }

        /// <summary>
        /// The number of vertices in post-order.
        /// </summary>
        public IEnumerable<int> PostOrder
        {
            get { return postOrder; }
        }

        /// <summary>
        /// The number of vertices in reverse post-order.
        /// </summary>
        public IEnumerable<int> ReversePostOrder
        {
            get
            {
                if (reversePostOrder != null)
                    return reversePostOrder;

                reversePostOrder = new Stack<int>();
                foreach (int v in postOrder)
                    reversePostOrder.Push(v);
                return reversePostOrder;
            }
        }

        /// <summary>
        /// Determines a depth-first order for a digraph G.
        /// </summary>
        /// <param name="G">The digraph.</param>
        public DepthFirstOrder(Digraph G)
        {
            preOrderNumber = new int[G.V];
            postOrderNumber = new int[G.V];
            postOrder = new Queue<int>();
            preOrder = new Queue<int>();
            marked = new bool[G.V];

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        /// <summary>
        /// Determines a depth-first order for the edge-weighted digraph G.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        public DepthFirstOrder(EdgeWeightedDigraph G)
        {
            preOrderNumber = new int[G.V];
            postOrderNumber = new int[G.V];
            postOrder = new Queue<int>();
            preOrder = new Queue<int>();
            marked = new bool[G.V];

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        /// <summary>
        /// Run DFS in digraph G from vertex v and compute pre-order and post-order.
        /// </summary>
        /// <param name="G">The digrpah.</param>
        /// <param name="v">The vertex from which to start the DFS.</param>
        private void Dfs(Digraph G, int v)
        {
            marked[v] = true;
            preOrderNumber[v] = preCounter++;
            preOrder.Enqueue(v);

            foreach (int w in G.Adjacent(v))
            {
                if (!marked[w])
                    Dfs(G, w);
            }

            postOrder.Enqueue(v);
            postOrderNumber[v] = postCounter++;
        }

        /// <summary>
        /// Run DFS in edge-weighted digraph G from vertex v and compute pre-order and post-order.
        /// </summary>
        /// <param name="G">The edge-weighted digrpah.</param>
        /// <param name="v">The vertex from which to start the DFS.</param>
        private void Dfs(EdgeWeightedDigraph G, int v)
        {
            marked[v] = true;
            preOrderNumber[v] = preCounter++;
            preOrder.Enqueue(v);
            foreach (DirectedEdge e in G.Adjacent(v))
            {
                int w = e.To();
                if (!marked[w])
                    Dfs(G, w);
            }
            postOrder.Enqueue(v);
            postOrderNumber[v] = postCounter++;
        }

        /// <summary>
        /// Returns the pre-order number of vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The pre-order number of vertex v.</returns>
        public int PreOrderNumberOf(int v) { return preOrderNumber[v]; }

        /// <summary>
        /// Returns the post-order number of vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The post-order number of vertex v.</returns>
        public int PostOrderNumberOf(int v) { return postOrderNumber[v]; }
    }
}