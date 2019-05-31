using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using Collections;

    /// <summary>
    /// The DirectedCycle class represests a data type for determing whether a digraph has a directed cycle.
    /// </summary>
    public class DirectedCycle
    {
        // marked[v] = true if vertex v has been marked, false otherwise.
        private bool[] marked;

        // Previous vertex on path to v.
        private int[] edgeTo;

        // onStack[v] = true if vertex v in on stack, false otherwise.
        private bool[] onStack;

        // The directed cycle.
        private Stack<int> cycle;

        /// <summary>
        /// Return true if the digrpah has a directed cycle, false otherwise.
        /// </summary>
        public bool HasCycle
        {
            get { return cycle != null; }
        }

        public DirectedCycle(Digraph G)
        {
            marked = new bool[G.V];
            edgeTo = new int[G.V];
            onStack = new bool[G.V];
            cycle = null;

            for (int v = 0; v < G.V; v++)
            {
                if ((!marked[v]) && (cycle == null))
                    Dfs(G, v);
            }
        }

        /// <summary>
        /// Check that algorithm computes either the topological order or finds a directed cycle.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="v">The vertex.</param>
        private void Dfs(Digraph G, int v)
        {
            onStack[v] = true;
            marked[v] = true;

            foreach (int w in G.Adjacent(v))
            {
                // Short circuit if directed cycle found.
                if (cycle != null)
                    return;

                // Found new vertex, so recur.
                else if (!marked[w])
                {
                    edgeTo[w] = v;
                    Dfs(G, w);
                }

                // Trace back directed cycle.
                else if (onStack[w])
                {
                    cycle = new Stack<int>();
                    for (int x = v; x != w; x = edgeTo[x])
                        cycle.Push(x);
                    cycle.Push(w);
                    cycle.Push(v);
                }
            }

            onStack[v] = false;
        }

        /// <summary>
        /// Return a directed cycle if the digraph has one, null otherwise.
        /// </summary>
        /// <returns>A directed cycle if the digraph has one, null otherwise.</returns>
        public IEnumerable<int> GetCycle() { return cycle; }
    }
}
