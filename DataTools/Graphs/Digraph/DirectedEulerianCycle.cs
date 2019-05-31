using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using Collections;
    using UndirectedGraph;

    /// <summary>
    /// The EulerianCycle class represents a data type for an Eulerian cycle or path in a digraph.
    /// An Eulerian cycle is a cycle (not necessarily simple) that use every edge in the digraph exactly once.
    /// </summary>
    public class DirectedEulerianCycle
    {
        // Eulerian cycle, null if no such cycle.
        private Stack<int> cycle;

        /// <summary>
        /// Computes an Eulerian cycle in the specified digraph, if one exists.
        /// </summary>
        /// <param name="G">The digraph.</param>
        public DirectedEulerianCycle(Digraph G)
        {
            cycle = null;

            // Short circuit.
            if (!HasEulerianCycle(G))
                return;

            // Create local view of adjacency lists, to iterate one vertex at a time.
            IEnumerator<int>[] adjacent = new IEnumerator<int>[G.V];
            for (int v = 0; v < G.V; v++)
                adjacent[v] = G.Adjacent(v).GetEnumerator();

            // Initialize stack with any non-isolated vertex.
            int s = NonIsolatedVertex(G);
            Stack<int> stack = new Stack<int>();
            stack.Push(s);

            // Greedily add to putative cycle, depth-first search style.
            cycle = new Stack<int>();
            while (!stack.IsEmpty)
            {
                int v = stack.Pop();
                while (adjacent[v].MoveNext())
                {
                    stack.Push(v);
                    v = adjacent[v].Current;
                }

                // Add vertex with no more leaving edges to cycle.
                cycle.Push(v);
            }

            if (cycle.Size != G.E + 1)
                cycle = null;
        }

        /// <summary>
        /// Returns the sequence of vertices on an Eulerian cycle, null if no such cycle.
        /// </summary>
        /// <returns>The sequence of vertices on an Eulerian cycle, null if no such cycle.</returns>
        public IEnumerable<int> Cycle() { return cycle; }

        /// <summary>
        /// Returns true if the digraph has an Eulerian cycle, false otherwise.
        /// </summary>
        /// <returns>true if the digraph has an Eulerian cycle, false otherwise.</returns>
        public bool HasEulerianCycle() { return cycle != null; }

        /// <summary>
        /// Returns any non-isolated vertex, -1 if no such vertex.
        /// </summary>
        /// <param name="G"></param>
        /// <returns></returns>
        private static int NonIsolatedVertex(Digraph G)
        {
            for (int v = 0; v < G.V; v++)
            {
                if (G.OutDegree(v) > 0)
                    return v;
            }
            return -1;
        }

        /// <summary>
        /// Determines whether a digraph has an Eulerian cycle using necessary and sufficient conditions without computeing the cycle itself.
        /// </summary>
        /// <param name="G"></param>
        /// <returns></returns>
        private static bool HasEulerianCycle(Digraph G)
        {
            // Condition 0: at least 1 edge.
            if (G.E == 0)
                return false;

            // Condition 1: InDegree(v) == OutDegree(v) for every vertex.
            for (int v = 0; v < G.V; v++)
            {
                if (G.InDegree(v) != G.OutDegree(v))
                    return false;
            }

            // Condition 2: the graph is connected when viewed as an undirected graph, ignoring isolated vertices.
            Graph H = new Graph(G.V);
            for (int v = 0; v < G.V; v++)
            {
                foreach (int w in G.Adjacent(v))
                    H.AddEdge(v, w);
            }

            // Check that all non-isolated vertices are connected.
            int s = NonIsolatedVertex(G);
            BreadthFirstPaths bfs = new BreadthFirstPaths(H, s);
            for (int v = 0; v < G.V; v++)
            {
                if ((H.Degree(v) > 0) && (!bfs.HasPathTo(v)))
                    return false;
            }

            return true;
        }
    }
}