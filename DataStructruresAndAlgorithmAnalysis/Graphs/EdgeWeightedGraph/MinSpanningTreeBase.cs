using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedUndirectedGraph
{
    using BasicDataStructures;

    /// <summary>
    /// The MinSpanningTreeBase class represents a data type for computing a mininum spanning tree in an edge-weighted graph.
    /// </summary>
    public abstract class MinSpanningTreeBase
    {
        // Edges in the mst.
        protected Queue<Edge> mst;

        /// <summary>
        /// Total weight of this MST.
        /// </summary>
        public double Weight { get; protected set; }

        /// <summary>
        /// Initializes the fields in the MinSpanningTreeBase.
        /// </summary>
        protected MinSpanningTreeBase() { Weight = 0; }

        /// <summary>
        /// Returns the edges in a mst (or forest) as an enumerator of edges.
        /// </summary>
        /// <returns>Edges in a mst (or forest) as an enumerator of edges.</returns>
        public virtual IEnumerable<Edge> Edges() { return mst; }
    }
}