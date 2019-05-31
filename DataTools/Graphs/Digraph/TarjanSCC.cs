using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using Collections;

    /// <summary>
    /// The TarjacSCC class represents a data type for determing the strongly connected components in a digraph.
    /// </summary>
    public class TarjanSCC : StronglyConnectedComponentsBase
    {
        private Stack<int> stack;

        // Low number of v.
        private int[] low;

        /// <summary>
        /// Computes the SCC of digraph G.
        /// </summary>
        /// <param name="G">The digraph.</param>
        public TarjanSCC(Digraph G)
            : base(G)
        {
            previous = 0;
            low = new int[G.V];
            stack = new Stack<int>();

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        private void Dfs(Digraph G, int v)
        {
            marked[v] = true;
            low[v] = previous++;

            int min = low[v];
            stack.Push(v);

            foreach (int w in G.Adjacent(v))
            {
                if (!marked[w])
                    Dfs(G, w);
                if (low[w] < min)
                    min = low[w];
            }

            if (min < low[v])
            {
                low[v] = min;
                return;
            }

            int u;
            do
            {
                u = stack.Pop();
                id[u] = Count;
                low[u] = G.V;
            }
            while (u != v);

            Count++;
        }
    }
}