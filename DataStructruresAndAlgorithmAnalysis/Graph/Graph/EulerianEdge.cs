using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.UndirectedGraph
{
    /// <summary>
    /// An un-directed edge, with a field to indicate whether the edge has been used.
    /// </summary>
    internal class EulerianEdge
    {
        private readonly int v;
        private readonly int w;

        /// <summary>
        /// True if this edge is used, false otherwise.
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// Create an edge of v-w.
        /// </summary>
        /// <param name="v">A vertex of this edge.</param>
        /// <param name="w">Another vertex of this edge.</param>
        public EulerianEdge(int v, int w)
        {
            this.v = v;
            this.w = w;
            IsUsed = false;
        }

        /// <summary>
        /// Returns the other vertex of the edge.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Returns the other vertex of the edge.</returns>
        public int Other(int vertex)
        {
            if (vertex == v)
                return w;
            if (vertex == w)
                return v;

            throw new ArgumentException(string.Format("Illegal endpoint: {0}", vertex));
        }

        /// <summary>
        /// Returns any non-isolated vertex in the graph G, -1 if no such vertex.
        /// </summary>
        /// <param name="G">The graph.</param>
        /// <returns>Returns any non-isolated vertex in the graph G, -1 if no such vertex.</returns>
        public static int NonIsolatedVertex(Graph G)
        {
            for (int v = 0; v < G.V; v++)
            {
                if (G.Degree(v) > 0)
                    return v;
            }
            return -1;
        }
    }
}
