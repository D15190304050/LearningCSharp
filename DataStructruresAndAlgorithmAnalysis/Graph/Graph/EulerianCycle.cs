using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.UndirectedGraph
{
    using BasicDataStructures;

    // Untested.
    /// <summary>
    /// The EulerianCycle class represents a data type for finding an Eulerian cycle or a path in graph.
    /// An Eulerian cycle is a cycle (not necessarily simple) that use every edge in the graph only once.
    /// </summary>
    public class EulerianCycle
    {
        // Eulerian cycle, null if no such cycle.
        private Stack<int> cycle;

        /// <summary>
        /// Computes an Eulerian cycle in the specific graph, if one exists.
        /// </summary>
        /// <param name="G">The graph.</param>
        public EulerianCycle(Graph G)
        {
            cycle = null;

            // Check whether G has an Eulerian cycle or not.
            if (!HasEulerianCycle(G))
                return;

            // Create local view of adjacency lists, to iterate one vertex at a time.
            // The helper EulerianEdge data type is used to avoid exploring both copies of an edge v-w.
            Queue<EulerianEdge>[] adjacent = new Queue<EulerianEdge>[G.V];
            for (int v = 0; v < G.V; v++)
                adjacent[v] = new Queue<EulerianEdge>();

            for (int v = 0; v < G.V; v++)
            {
                // Add the edge of self loop when the selfLoop is even, and ignore the same edge next time.
                int selfLoops = 0;

                foreach (int w in G.Adjacent(v))
                {
                    // Careful with self loops.
                    if (v == w)
                    {
                        if (selfLoops % 2 == 0)
                        {
                            EulerianEdge e = new EulerianEdge(v, w);
                            adjacent[v].Enqueue(e);
                            adjacent[w].Enqueue(e);
                        }
                        selfLoops++;
                    }

                    // To include the same edge once, add it into adjacency lists when v < w.
                    else if (v < w)
                    {
                        EulerianEdge e = new EulerianEdge(v, w);
                        adjacent[v].Enqueue(e);
                        adjacent[w].Enqueue(e);
                    }
                }
            }

            // Initialize stack with any non-isolated vertex.
            int s = EulerianEdge.NonIsolatedVertex(G);
            Stack<int> stack = new Stack<int>();
            stack.Push(s);

            // Greedily search through edges in iterative DFS style.
            cycle = new Stack<int>();
            while (!stack.IsEmpty)
            {
                int v = stack.Pop();
                while (!adjacent[v].IsEmpty)
                {
                    EulerianEdge e = adjacent[v].Dequeue();
                    if (e.IsUsed)
                        continue;
                    e.IsUsed = true;

                    // Push v into stack to iteratively access the next edge stored in adjacent[v].
                    stack.Push(v);

                    // To access edges stores in another adjacency list by vertex w.
                    v = e.Other(v);
                }

                // Push vertex with no more leaving edges to cycle.
                cycle.Push(v);
            }

            // Check if all edges are used.
            if (cycle.Size != G.E + 1)
                cycle = null;
        }

        /// <summary>
        /// True if the graph has an Eulerian cycle, false otherwise.
        /// </summary>
        public bool HasEulerianCycle()
        {
            return cycle != null;
        }

        /// <summary>
        /// Returns a sequence of vertices on an Eulerian cycle, null if no such cycle.
        /// </summary>
        /// <returns>Returns a sequence of vertices on an Eulerian cycle, null if no such cycle.</returns>
        public IEnumerable<int> GetCycle() { return cycle; }

        /* This code below is solely testing the correctness of the data type. */
        /// <summary>
        /// Determines whether a graph has an Eulerian cycle using necessary and sufficient conditions (without computing the cycle itself):
        /// -At least one edge.
        /// -G.Degree(v) is even for every vertex v.
        /// -The graph is connected (ignoring isolated vertices).
        /// </summary>
        /// <param name="G">The graph.</param>
        /// <returns>True if the graph G has an Eulerian cycle, false otherwise.</returns>
        private static bool HasEulerianCycle(Graph G)
        {
            // Condition 0: at least 1 edge.
            if (G.E == 0)
                return false;

            // Condition 1: G.Degree(v) is even for every vertex.
            for (int v = 0; v < G.V; v++)
            {
                if (G.Degree(v) % 2 != 0)
                    return false;
            }

            // Condition 2: graph is connected, ignoring isolated vertices.
            int s = EulerianEdge.NonIsolatedVertex(G);
            BreadthFirstPaths bfs = new BreadthFirstPaths(G, s);
            for (int v = 0; v < G.V; v++)
            {
                if (G.Degree(v) > 0 && !bfs.HasPathTo(v))
                    return false;
            }

            return true;
        }
    }
}
