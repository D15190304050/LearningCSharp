using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph
{
    /// <summary>
    /// The TransitiveClosure class represents a data type for computing the transitive closure of a digraph.
    /// </summary>
    public class TransitiveClosure
    {
        // tc[v] = reachable from v.
        private DepthFirstDirectedPaths[] tc;

        /// <summary>
        /// Computes the transitive closure of the digraph G.
        /// </summary>
        /// <param name="G">The digraph.</param>
        public TransitiveClosure(Digraph G)
        {
            tc = new DepthFirstDirectedPaths[G.V];
            for (int v = 0; v < G.V; v++)
                tc[v] = new DepthFirstDirectedPaths(G, v);
        }

        /// <summary>
        /// Returns true if there is a directed path from vertex v to vertex w in the digraph, false otherwise.
        /// </summary>
        /// <param name="v">The source vertex.</param>
        /// <param name="w">The target vertex.</param>
        /// <returns>True if there is a directed path from vertex v to vertex w in the digraph, false otherwise.</returns>
        public bool Reachable(int v, int w) { return tc[v].HasPathTo(w); }
    }
}
