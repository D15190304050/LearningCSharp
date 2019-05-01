using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedUndirectedGraph
{
    /// <summary>
    /// The Edge class represents a weighted edge in an edge weighted graph.
    /// Each edge consists of 2 integers (naming the 2 vertices) and a double-value weight.
    /// </summary>
    public class Edge : IComparable<Edge>
    {
        // A vertex.
        private int v;

        // The other vertex.
        private int w;

        /// <summary>
        /// The weight of this edge.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Initializes an edge between vertex v and vertex w of the given weight.
        /// </summary>
        /// <param name="v">A vertex of this edge.</param>
        /// <param name="w">The other vertex of this edge.</param>
        /// <param name="weight">The weight of this edge.</param>
        public Edge(int v, int w, double weight)
        {
            if (v < 0 || w < 0)
                throw new ArgumentOutOfRangeException("Vertex name must be a non-negative integer.");
            if (double.IsNaN(weight))
                throw new ArgumentException("Weight is NaN.");

            this.v = v;
            this.w = w;
            Weight = weight;
        }

        /// <summary>
        /// Returns either end-point of this edge.
        /// </summary>
        /// <returns>Either end-point of this edge.</returns>
        public int Either() { return v; }

        /// <summary>
        /// Returns the end-point of this edge that is different from the given vertex.
        /// </summary>
        /// <param name="v">One end-point of this edge.</param>
        /// <returns>The end-point of this edge that is different from the given vertex.</returns>
        public int Other(int v)
        {
            if (v == this.v)
                return w;
            else if (v == w)
                return this.v;
            else
                throw new ArgumentOutOfRangeException("Illegal end-point.");
        }

        /// <summary>
        /// Compare 2 edges by weight.
        /// </summary>
        /// <param name="that">The other edge.</param>
        /// <returns>
        /// A negative integer, 0, or positive integer depending on whether the weight of this is less than,
        /// equal to, or greater than the argument edge.
        /// </returns>
        public int CompareTo(Edge that)
        {
            double epsilon = 1E-4;

            if (Weight - that.Weight < epsilon)
                return -1;
            else if (Weight - that.Weight > epsilon)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Returns a string representation of this edge.
        /// </summary>
        /// <returns>T string representation of this edge.</returns>
        public override string ToString()
        {
            return string.Format("{0}-{1} {2:F5}", v, w, Weight);
        }
    }
}