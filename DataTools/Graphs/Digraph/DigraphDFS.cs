using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    /// <summary>
    /// The DigraphDFS class represents a data type for determing the vertices reachable from a given source vertex in a digraph.
    /// </summary>
    public class DirectedDFS
    {
        /// <summary>
        /// marked[v] = true if v is reachable from source, false other wise.
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// Number of vertices reachable from source.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Couputes the vertices in digraph that are reachable from the source vertex.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="source">The source vertex.</param>
        public DirectedDFS(Digraph G, int source)
        {
            marked = new bool[G.V];
            Dfs(G, source);
        }

        /// <summary>
        /// Couputes the vertices in digraph that are reachable from the source vertex.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="source">The source vertices.</param>
        public DirectedDFS(Digraph G, IEnumerable<int> sources)
        {
            marked = new bool[G.V];
            foreach (int v in sources)
            {
                if (!marked[v])
                    Dfs(G, v);
            }
        }

        /// <summary>
        /// Computes the vertices in digraph that are reachable from the source vertex.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="v">The source vertex.</param>
        private void Dfs(Digraph G, int v)
        {
            marked[v] = true;
            foreach (int w in G.Adjacent(v))
            {
                if (!marked[w])
                    Dfs(G, w);
            }
        }

        /// <summary>
        /// Returns true if there is a path from the source vertex and vertex v, false otherwise.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>True if there is a path from the source vertex and vertex v, false otherwise.</returns>
        public bool HasPathTo(int v) { return marked[v]; }
    }
}
