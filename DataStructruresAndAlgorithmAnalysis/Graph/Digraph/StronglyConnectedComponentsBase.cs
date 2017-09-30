using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph
{
    /// <summary>
    /// The StronglyConnectedCompontesBase class represents a data type for determining strongly connected components in a digraph.
    /// </summary>
    public abstract class StronglyConnectedComponentsBase
    {
        // True if marked[v] is visited, false otherwise.
        protected bool[] marked;

        // ID of SCC containing v.
        protected int[] id;

        // Pre-order number counter.
        protected int previous;

        /// <summary>
        /// The number of SCCs.
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Initialize the variable of array type.
        /// </summary>
        /// <param name="G">The digraph.</param>
        protected StronglyConnectedComponentsBase(Digraph G)
        {
            marked = new bool[G.V];
            id = new int[G.V];
        }

        /// <summary>
        /// Returns true if vertex v and vertex w are in the same SCC, false otherwise.
        /// </summary>
        /// <param name="v">A vertex.</param>
        /// <param name="w">The other vertex.</param>
        /// <returns>True if vertex v and vertex w are in the same SCC, false otherwise.</returns>
        public bool StronglyConnected(int v, int w) { return id[v] == id[w]; }

        /// <summary>
        /// Returns the ID of the SCC containing vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The ID of the SCC containing vertex v.</returns>
        public int ID(int v) { return id[v]; }
    }
}