using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using BasicDataStructures;

    public class DepthFirstDirectedPaths
    {
        // marked[v] = true if v is reachable from s.
        private bool[] marked;

        // edgeTo[v] = last edge on path from s to v.
        private int[] edgeTo;

        // Source vertex.
        private readonly int source;

        /// <summary>
        /// The DepthFirstDirectedPaths class represents a data type for finding directed paths from a source vertex to every other vertex in the digraph.
        /// </summary>
        /// <param name="G"></param>
        public DepthFirstDirectedPaths(Digraph G, int source)
        {
            marked = new bool[G.V];
            edgeTo = new int[G.V];
            this.source = source;
            Dfs(G, source);
        }

        /// <summary>
        /// Compute a directed path from source to every other vertex in digraph G.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        private void Dfs(Digraph G, int v)
        {
            marked[v] = true;
            foreach (int w in G.Adjacent(v))
            {
                if (!marked[w])
                {
                    edgeTo[w] = v;
                    Dfs(G, w);
                }
            }
        }

        /// <summary>
        /// Return true if there is a path from the source vertex to vertex v, false otherwise.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>True if there is a path from the source vertex to vertex v, false otherwise.</returns>
        public bool HasPathTo(int v) { return marked[v]; }

        public IEnumerable<int> PathTo(int v)
        {
            if (!marked[v])
                return null;

            Stack<int> path = new Stack<int>();
            for (int x = v; x != source; x = edgeTo[x])
                path.Push(x);
            path.Push(source);

            return path;
        }
    }
}
