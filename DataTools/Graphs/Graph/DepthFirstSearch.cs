using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    public class DepthFirstSearch : GraphSearchBase
    {

        /// <summary>
        /// Compute the vertices in graph G that are connected to the source vertex.
        /// </summary>
        /// <param name="G">The graph.</param>
        /// <param name="source">The source vertex.</param>
        public DepthFirstSearch(Graph G, int source)
            : base(G, source)
        { Dfs(G, source); }

        /// <summary>
        /// Depth first search from v.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        private void Dfs(Graph G, int v)
        {
            Count++;
            Marked[v] = true;
            foreach (int w in G.Adjacent(v))
            {
                if (!Marked[w])
                    Dfs(G, w);
            }
        }
    }
}
