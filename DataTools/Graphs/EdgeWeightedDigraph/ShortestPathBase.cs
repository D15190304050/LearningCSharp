using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    using Collections;

    /// <summary>
    /// The ShortestPathBase class represents a data type for solving the single-source shortest paths problems in edge-weighted digraphs.
    /// </summary>
    public abstract class ShortestPathBase : EdgeWeightedPathsBase
    {
        /// <summary>
        /// Initializes the protected field for derived class to compute the shortest paths.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        /// <param name="source">The source vertex.</param>
        protected ShortestPathBase(EdgeWeightedDigraph G, int source)
            : base(G, source)
        {
            // Initializes the distance info.
            for (int v = 0; v < G.V; v++)
                distanceTo[v] = double.PositiveInfinity;
            distanceTo[source] = 0;
        }
    }
}