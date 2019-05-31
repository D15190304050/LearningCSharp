using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    using Collections;

    /// <summary>
    /// The EdgeWeightedDirectedCycle class represents a data type for determining whether an edge-weighted digraph has a directed cycle.
    /// </summary>
    public class EdgeWeightedDirectedCycle
    {
        // marked[v] = true if v is marked, false otherwise.
        private bool[] marked;

        // edgeTo[v] = previous on path to v.
        private DirectedEdge[] edgeTo;

        // onStack[v] = true if v is on the stack, false otherwise.
        private bool[] onStack;

        // Directed cycle, null if no such cycle.
        private Stack<DirectedEdge> cycle;

        /// <summary>
        /// Returns true if the edge-weighted digraph has a directed cycle, false otherwise.
        /// </summary>
        public bool HasCycle
        {
            get { return cycle != null; }
        }

        /// <summary>
        /// Determines whether the edge-weighted digraph G has a directed cycle, and ,if so, finds such a cycle.
        /// </summary>
        /// <param name="G">The edge-weighted cycle.</param>
        public EdgeWeightedDirectedCycle(EdgeWeightedDigraph G)
        {
            marked = new bool[G.V];
            edgeTo = new DirectedEdge[G.V];
            onStack = new bool[G.V];
            cycle = null;

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        /// <summary>
        /// Check that algorithm computes either the topological order or finds a directed cycle.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        /// <param name="v">The vertex.</param>
        private void Dfs(EdgeWeightedDigraph G, int v)
        {
            onStack[v] = true;
            marked[v] = true;
            foreach (DirectedEdge e in G.Adjacent(v))
            {
                int w = e.To();

                // Short circuit if directed cycle found.
                if (cycle != null)
                    return;

                // Found new vertex, so recur.
                else if (!marked[w])
                {
                    edgeTo[w] = e;
                    Dfs(G, w);
                }

                // Track back directed cycle.
                else if (onStack[w])
                {
                    cycle = new Stack<DirectedEdge>();

                    DirectedEdge f = e;
                    while (f.From() != w)
                    {
                        cycle.Push(f);
                        f = edgeTo[f.From()];
                    }
                    cycle.Push(f);

                    return;
                }
            }

            onStack[v] = false;
        }

        /// <summary>
        /// Returns a directed cycle if the edge-weighted cycle has one, null otherwise.
        /// </summary>
        /// <returns>A directed cycle if the edge-weighted cycle has one, null otherwise.</returns>
        public IEnumerable<DirectedEdge> GetCycle() { return cycle; }
    }
}