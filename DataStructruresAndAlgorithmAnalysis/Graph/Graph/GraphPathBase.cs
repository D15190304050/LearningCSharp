using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.UndirectedGraph
{
    public abstract class GraphPathBase : GraphSearchBase
    {
        // Last edge on path s-v.
        protected int[] edgeTo;

        /// <summary>
        /// The source vertex of this path search.
        /// </summary>
        public int Source { get; protected set; }

        public GraphPathBase(Graph G, int source)
            : base(G, source)
        {
            Source = source;
            edgeTo = new int[G.V];
        }

        /// <summary>
        /// Returns a path between the source vertex and vertex v, or null if no such path.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public abstract IEnumerable<int> PathTo(int v);
    }
}
