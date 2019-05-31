using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    using Collections;

    /// <summary>
    /// The Bipartite class represent a data type for determing whether an un-directed graph is bipartite or whether it has an odd-length cycle.
    /// This implementation uses DFS.
    /// Untested.
    /// </summary>
    public class Bipartite
    {
        // True if the vertex has been visited in DFS, false otherwise.
        private bool[] marked;

        // Last edge on path to v.
        private int[] edgeTo;

        /// <summary>
        /// Returns an odd-length cycle if the graph is not bipartite, null otherwise.
        /// </summary>
        public Stack<int> OddCycle { get; private set; }

        /// <summary>
        /// True if the graph is bipartite, false otherwise.
        /// </summary>
        public bool IsBipartite { get; private set; }

        // color[v] gives vertices on one side of bipartition.
        private bool[] color;

        /// <summary>
        /// Determines whether an un-directed graph is bipartite and finds either a bipartition or an odd-length cycle.
        /// </summary>
        /// <param name="G">The graph.</param>
        public Bipartite(Graph G)
        {
            // Initialize all fields and properties.
            IsBipartite = true;
            color = new bool[G.V];
            marked = new bool[G.V];
            edgeTo = new int[G.V];
            OddCycle = null;

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        /// <summary>
        /// Depth first search. Fill the OddCycle and return immediately if an odd-length cycle if found, continue to recur otherwise.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        private void Dfs(Graph G, int v)
        {
            marked[v] = true;

            foreach (int w in G.Adjacent(v))
            {
                // Short circuit if odd-length cycle found.
                if (OddCycle != null)
                    return;

                // Find un-colored vertex, so recur.
                if (!marked[v])
                {
                    edgeTo[w] = v;
                    color[w] = !color[v];
                    Dfs(G, w);
                }

                // If v-w create an odd-length cycle, find it.
                else if (color[w] == color[v])
                {
                    IsBipartite = false;
                    OddCycle = new Stack<int>();

                    // Don't need this unless you want to include start vertex twice.
                    OddCycle.Push(w);

                    for (int x = v; x != w; x = edgeTo[x])
                        OddCycle.Push(x);
                    OddCycle.Push(w);
                }
            }
        }

        /// <summary>
        /// Return he side of the bipartite which a specific vertex is on.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Color(int v)
        {
            if (!IsBipartite)
                throw new MemberAccessException("Graph is not bipartite.");
            return color[v];
        }
    }
}
