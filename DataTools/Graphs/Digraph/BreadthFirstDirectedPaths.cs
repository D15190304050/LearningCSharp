using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using Collections;

    /// <summary>
    /// The BreadthFirstDirectedPaths class represents a data type for finding shortest paths (number of edges) from a source vertex to every other vertices in the digraph.
    /// </summary>
    public class BreadthFirstDirectedPaths
    {
        // marked[v] = true if there is a path from source to v, false otherwise.
        private bool[] marked;

        // edgeTo[v] = last path on shortest source->v path.
        private int[] edgeTo;

        // distanceTo[v] = length of shortest source->v path.
        private int[] distanceTo;

        /// <summary>
        /// Computes the shortest path from source and every other vertex in digraph G
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="source">The source vertex.</param>
        public BreadthFirstDirectedPaths(Digraph G, int source)
        {
            marked = new bool[G.V];
            edgeTo = new int[G.V];
            distanceTo = new int[G.V];
            for (int v = 0; v < G.V; v++)
                distanceTo[v] = int.MaxValue;

            Bfs(G, source);
        }

        /// <summary>
        /// Breath first search from a single source.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="source">The source vertex.</param>
        private void Bfs(Digraph G, int source)
        {
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);

            marked[source] = true;
            distanceTo[source] = 0;

            while (!queue.IsEmpty)
            {
                int v = queue.Dequeue();
                foreach (int w in G.Adjacent(v))
                {
                    if (!marked[w])
                    {
                        marked[w] = true;
                        edgeTo[w] = v;
                        distanceTo[w] = distanceTo[v] + 1;
                        queue.Enqueue(w);
                    }
                }
            }
        }

        /// <summary>
        /// Return true if there is a path from the source vertex to vertex v, false otherwise.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns></returns>
        public bool HasPathTo(int v) { return marked[v]; }

        /// <summary>
        /// Return the number of edges in a shortest path from the source vertex to vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The number of edges in a shortest path from the source vertex to vertex v.</returns>
        public int DistanceTo(int v) { return distanceTo[v]; }

        /// <summary>
        /// Return a shortest path from the source vertex to vertex v, null if no such path.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>A shortest path from the source vertex to vertex v, null if no such path.</returns>
        public IEnumerable<int> PathTo(int v)
        {
            if (!marked[v])
                return null;

            Stack<int> path = new Stack<int>();
            int x;
            for (x = v; distanceTo[x] != 0; x = edgeTo[x])
                path.Push(x);
            path.Push(x);

            return path;
        }
    }
}