using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using BasicDataStructures;

    /// <summary>
    /// The GabowSCC class represents a data type for determing SCCs in a digraph.
    /// </summary>
    public class GabowSCC : StronglyConnectedComponentsBase
    {
        private Stack<int> stack1;
        private Stack<int> stack2;

        // preOrder[v] = preOrder of v.
        private int[] preOrder;

        /// <summary>
        /// Computes the SCCs of the digraph G.
        /// </summary>
        /// <param name="G">The digraph.</param>
        public GabowSCC(Digraph G)
            : base(G)
        {
            stack1 = new Stack<int>();
            stack2 = new Stack<int>();
            preOrder = new int[G.V];

            for (int v = 0; v < G.V; v++)
                id[v] = -1;

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        private void Dfs(Digraph G, int v)
        {
            marked[v] = true;
            preOrder[v] = previous++;
            stack1.Push(v);
            stack2.Push(v);

            foreach (int w in G.Adjacent(v))
            {
                if (!marked[w])
                    Dfs(G, w);
                else if (id[w] == -1)
                {
                    while (preOrder[stack2.Peek()] > preOrder[w])
                        stack2.Pop();
                }
            }

            // Found SCC containing v.
            if (stack2.Peek() == v)
            {
                stack2.Pop();
                int u;
                do
                {
                    u = stack1.Pop();
                    id[u] = Count;
                }
                while (u != v);
                Count++;
            }
        }
    }
}