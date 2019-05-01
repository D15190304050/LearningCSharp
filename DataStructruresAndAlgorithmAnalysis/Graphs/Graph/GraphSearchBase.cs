using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    public abstract class GraphSearchBase
    {
        // True if there is a path from source to some specific vertex.
        protected bool[] Marked {  get;  set; }

        /// <summary>
        /// Number of vertices connect to source.
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Is there a path between the source vertex and vertex v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool HasPathTo(int v) { return Marked[v]; }

        public GraphSearchBase(Graph G, int source)
        {
            Marked = new bool[G.V];
            Count = 0;
        }
    }
}
