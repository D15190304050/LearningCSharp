using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph
{
    using BasicDataStructures;
    using UndirectedGraph;

    /// <summary>
    /// The DirectedEulerianPath class represents a data type for finding an Eulerian path in a digraph.
    /// An Eulerian path is a path (not necessarily simple) that uses every edge in the digraph exactly once.
    /// </summary>
    public class DirectedEulerianPath
    {
        // Eulerian path, null if no such path.
        private Stack<int> path;

        /// <summary>
        /// Computes an Eulerian path in the specified digraph, if one exists.
        /// </summary>
        public DirectedEulerianPath(Digraph G)
        {
            path = null;

            // Short circuit.
            if (!HasEulerianPath(G))
                return;

            // Find a vertex from which to start potential Eulerian path:
            // A vertex with OutDegree(v) > InDegree(v) if exists.
            // Otherwise a vertex with OutDegree(v) > 0.
            int s = NonIsolatedVertex(G);
            for (int v = 0; v < G.V; v++)
            {
                if (G.OutDegree(v) > G.InDegree(v))
                    s = v;
            }

            // Sepcial case for digraph with 0 edges (has a degenerate Euelrian path).
            if (s == -1)
                s = 0;

            // Create local view of adjacency lists to iterate one vertex at a time.
            IEnumerator<int>[] adjacent = new IEnumerator<int>[G.V];
            for (int v = 0; v < G.V; v++)
                adjacent[v] = G.Adjacent(v).GetEnumerator();

            // Greedy add to cycle, depth-first search style.
            Stack<int> stack = new Stack<int>();
            stack.Push(s);
            path = new Stack<int>();
            while (!stack.IsEmpty)
            {
                int v = stack.Pop();
                while (adjacent[v].MoveNext())
                {
                    stack.Push(v);
                    v = adjacent[v].Current;
                }

                // Puch vertex with no more available edges to path.
                path.Push(v);
            }

            // Check if all edges have been used.
            if (path.Size != G.E + 1)
                path = null;
        }

        /// <summary>
        /// Returns the sequence of vertices on an Eulerian path.
        /// </summary>
        /// <returns>The sequence of vertices on an Eulerian path.</returns>
        public IEnumerable<int> Path() { return path; }

        /// <summary>
        /// Returns true if the digraph has an Eulerian path, false otherwise.
        /// </summary>
        /// <returns>True if the digraph has an Eulerian path, false otherwise.</returns>
        public bool HasEulerianPath() { return path != null; }

        /// <summary>
        /// Returns any non-isolated vertex, -1 if no such vertex.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <returns>Any non-isolated vertex, -1 if no such vertex.</returns>
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
        /// Determines whether a digraph has an Eulerian path using necessary and sufficient conditions without computing the path itself.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <returns>True if the digraph has an Eulerian path, falsewise.</returns>
        private static bool HasEulerianPath(Digraph G)
        {
            if (G.E == 0)
                return true;

            // Condition 0: InDegree(v) == OutDegree(v) for every verex.
            // Except one vertex may have OutDegree(v) == InDegree(v) + 1.
            int deficit = 0;
            for (int v = 0; v < G.V; v++)
            {
                if (G.OutDegree(v) > G.InDegree(v))
                    deficit += (G.OutDegree(v) - G.InDegree(v));
            }
            if (deficit > 1)
                return false;

            // Condition 2: graph is connected, when viewed as an undirected graph (igonring isolated vertices).
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