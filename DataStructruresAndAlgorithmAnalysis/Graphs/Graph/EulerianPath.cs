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
    /// The EulerianPath class represents a data type for finding an Eulerian path in a graph.
    /// An Eulerian path is a path (not necessarily simple) that uses every edge in the graph only once.
    /// </summary>
    public class EulerianPath
    {
        // Eulerian path, null if no such path.
        private Stack<int> path;

        /// <summary>
        /// Computes an Eulerian path in the specific graph, if one exists.
        /// </summary>
        /// <param name="G">The graph.</param>
        public EulerianPath(Graph G)
        {
            path = null;

            // Short circuit.
            if (!HasEulerianPath(G))
                return;

            // Find vertex from which to start potential Eulerian path:
            // a vertex v with odd G.Degree(v) if exists;
            // otherwise a vertex with G.Degree(v) > 0.
            int s = EulerianEdge.NonIsolatedVertex(G);
            for (int v = 0; v < G.V; v++)
            {
                if (G.Degree(v) % 2 != 0)
                    s = v;
            }

            // Sepcial case for graph with 0 degrees (has a degenerate Eulerian path).
            if (s == -1)
                s = 0;

            // Create a local view of adjacency lists, to iterate one vertex at a time.
            // The helper EulerianEdge data type is used avoid exploring both copies of an edge v-w.
            Queue<EulerianEdge>[] adjacent = new Queue<EulerianEdge>[G.V];
            for (int v = 0; v < G.V; v++)
                adjacent[v] = new Queue<EulerianEdge>();

            for (int v = 0; v < G.V; v++)
            {
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
                    else if (v < w)
                    {
                        EulerianEdge e = new EulerianEdge(v, w);
                        adjacent[v].Enqueue(e);
                        adjacent[w].Enqueue(e);
                    }
                }
            }

            // Initialize stack with any non-isolated vertex.
            Stack<int> stack = new Stack<int>();
            stack.Push(s);

            // Greedy search through edges in iterative DFS style.
            path = new Stack<int>();
            while (!stack.IsEmpty)
            {
                int v = stack.Pop();
                while (!adjacent[v].IsEmpty)
                {
                    EulerianEdge e = adjacent[v].Dequeue();
                    if (e.IsUsed)
                        continue;
                    e.IsUsed = true;
                    stack.Push(v);
                    v = e.Other(v);
                }
                // Push vertex with no more leaving edges to path.
                path.Push(v);
            }

            // Check if all edges are used.
            if (path.Size != G.E + 1)
                path = null;
        }

        /// <summary>
        /// Returns the sequence of vertices in an Eulerian path, null if no such path.
        /// </summary>
        /// <returns>Returns the sequence of vertices in an Eulerian path, null if no such path.</returns>
        public IEnumerable<int> Path() { return path; }

        /// <summary>
        /// True if the graph has an Eulerian path, false otherwise.
        /// </summary>
        public bool HasEulerianPath() { return path != null; }

        /* The code below is solely for testing correctness of the data type. */
        /// <summary>
        /// <para>Determines whether a graph has an Eulerian path using necessary and sufficient conditions (without computing the graph itself):</para>
        /// <para>-G.Degree(v) is even for every vertex, except for possibly 2.</para>
        /// <para>-The graph is connected (ignoring isolated vertices).</para>
        /// </summary>
        /// <param name="G"></param>
        /// <returns></returns>
        private static bool HasEulerianPath(Graph G)
        {
            if (G.E == 0)
                return true;

            // Condition 0: G.Degree(v) is even except for possibly 2.
            int oddDegreeVertices = 0;
            for (int v = 0; v < G.V; v++)
            {
                if (G.Degree(v) % 2 == 1)
                    oddDegreeVertices++;
            }
            if (oddDegreeVertices > 2)
                return false;

            // Condition 1: graph is connected, igonring isolated vertices.
            int source = EulerianEdge.NonIsolatedVertex(G);
            BreadthFirstPaths bfs = new BreadthFirstPaths(G, source);
            for (int v = 0; v < G.V; v++)
            {
                if (G.Degree(v) > 0 && !bfs.HasPathTo(v))
                    return false;
            }

            return true;
        }
    }
}