using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    using Collections;

    /// <summary>
    /// The Cycle class represents a data type for determining whether an un-directed graph has a cycle.
    /// </summary>
    public class Cycle
    {
        private bool[] marked;
        private int[] edgeTo;
        private Stack<int> cycle;

        /// <summary>
        /// Return true if the graph has a cycle, false otherwise.
        /// </summary>
        /// <returns>Return true if the graph has a cycle, false otherwise.</returns>
        public bool HasCycle
        {
            get { return cycle != null; }
        }

        /// <summary>
        /// Determins whether the un-directed graph G has a cycle, and if so, find such a cycle.
        /// </summary>
        /// <param name="G">The graph.</param>
        public Cycle(Graph G)
        {
            marked = null;
            edgeTo = null;
            cycle = null;

            if (HasSelfLoop(G))
                return;
            if (HasParallelEdges(G))
                return;

            marked = new bool[G.V];
            edgeTo = new int[G.V];
            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, -1, v);
            }
        }

        /// <summary>
        /// Return true if this graph has a self loop, false otherwise.
        /// Side effect: initialize cycle to be self loop.
        /// </summary>
        /// <param name="G">The graph.</param>
        /// <returns>Return true if this graph has a self loop, false otherwise.</returns>
        private bool HasSelfLoop(Graph G)
        {
            for (int v = 0; v < G.V; v++)
            {
                foreach (int w in G.Adjacent(v))
                {
                    if (v == w)
                    {
                        cycle = new Stack<int>();
                        cycle.Push(v);
                        cycle.Push(v);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Return true if the graph has 2 parallel edges.
        /// Side effect: initialize cycle to be 2 parallel edges.
        /// </summary>
        /// <param name="G">The graph.</param>
        /// <returns>Return true if the graph has 2 parallel edges.</returns>
        private bool HasParallelEdges(Graph G)
        {
            marked = new bool[G.V];

            for (int v = 0; v < G.V; v++)
            {
                // Check for parallel edges incident to v.
                foreach (int w in G.Adjacent(v))
                {
                    if (marked[w])
                    {
                        cycle = new Stack<int>();
                        cycle.Push(v);
                        cycle.Push(w);
                        cycle.Push(v);
                        return true;
                    }
                    marked[w] = true;
                }

                // Reset so marked[v] = false for all v.
                foreach (int w in G.Adjacent(v))
                    marked[w] = false;
            }

            return false;
        }

        /// <summary>
        /// Returns a cycle in the graph G.
        /// </summary>
        /// <returns>Returns a cycle in the graph G if G has a cycle, and null otherwise.</returns>
        public IEnumerable<int> GetCycle() { return cycle; }

        /// <summary>
        /// Run depth-first search for the graph G. Fill the cycle if find one.
        /// </summary>
        /// <param name="G">The graph.</param>
        /// <param name="u">The vertex on one edge.</param>
        /// <param name="v">The vertex on the same edge.</param>
        private void Dfs(Graph G, int u, int v)
        {
            marked[v] = true;

            foreach (int w in G.Adjacent(v))
            {
                // Short circuit if cycle already found.
                if (cycle != null)
                    return;

                if (!marked[w])
                {
                    edgeTo[w] = v;
                    Dfs(G, v, w);
                }

                // Check for cycle but dis-regard reverse of edge leading to v.
                else if (w != u)
                {
                    cycle = new Stack<int>();
                    for (int x = v; x != w; x = edgeTo[x])
                        cycle.Push(x);

                    cycle.Push(w);
                    cycle.Push(v);
                }
            }
        }
    }
}
