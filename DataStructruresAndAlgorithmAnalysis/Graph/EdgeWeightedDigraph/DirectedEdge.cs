using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedDirectedGraph
{
    /// <summary>
    /// The DirectedEdge class represents a weighted edge in an EdgeWeightedDigraph.
    /// </summary>
    public class DirectedEdge
    {
        // 2 vertices of this directed edge.
        private readonly int v;
        private readonly int w;

        /// <summary>
        /// The weight of this directed edge.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Initializes a directed edge from vertex v to vertex w (v->w) with the given weight.
        /// </summary>
        /// <param name="v">The tail vertex.</param>
        /// <param name="w">The head vertex.</param>
        /// <param name="weight">The weight of this directed edge.</param>
        public DirectedEdge(int v, int w, double weight)
        {
            if ((v < 0) || (w < 0))
                throw new ArgumentOutOfRangeException("Vertex names must be non-negative integers.");
            if (double.IsNaN(weight))
                throw new ArgumentException("Weight is NaN.");

            this.v = v;
            this.w = w;
            Weight = weight;
        }

        /// <summary>
        /// Returns the tail vertex of this directred edge.
        /// </summary>
        /// <returns>The tail vertex of this directred edge.</returns>
        public int From() { return v; }

        /// <summary>
        /// Returns the head vertex of this directed vertex.
        /// </summary>
        /// <returns>The head vertex of this directed vertex.</returns>
        public int To() { return w; }

        /// <summary>
        /// Returns a string representation of this directed edge.
        /// </summary>
        /// <returns>A string representation of this directed edge.</returns>
        public override string ToString() { return string.Format("{0}->{1} {2,5:F2}", v, w, Weight); }
    }
}