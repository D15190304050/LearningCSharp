using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    public class ConnectedComponents
    {
        // True if a specific vertex is marked, false otherwise.
        private bool[] marked;

        /// <summary>
        /// The component id of the connected component containing a specific vertex.
        /// </summary>
        public int[] Id { get; private set; }

        /// <summary>
        /// The number of vertex in the connected component containing a specific vertex.
        /// </summary>
        public int[] Size { get; private set; }

        /// <summary>
        /// The number of connected components in the graph.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Compute the connected components of the un-directed graph G.
        /// </summary>
        /// <param name="G">The un-directed graph.</param>
        public ConnectedComponents( Graph G)
        {
            marked = new bool[G.V];
            Id = new int[G.V];
            Size = new int[G.V];

            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                {
                    Dfs(G, v);
                    Count++;
                }
            }
        }

        /// <summary>
        /// Depth-first search.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        private void Dfs(Graph G, int v)
        {
            marked[v] = true;
            Id[v] = Count;
            Size[Count]++;
            foreach (int w in G.Adjacent(v))
            {
                if (!marked[w])
                    Dfs(G, w);
            }
        }

        /// <summary>
        /// Return trur if vertices v and w are in the same connected component, false otherwise.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool Connected(int v, int w) { return Id[v] == Id[w]; }
    }
}