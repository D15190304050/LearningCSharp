using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// The VisualEdge class represents the visual edge and its weight of a graph.
    /// </summary>
    public class VisualEdge : IComparable<VisualEdge>
    {
        /// <summary>
        /// A vertex of this edge.
        /// </summary>
        private int v;

        /// <summary>
        /// Another vertex of this edge.
        /// </summary>
        private int w;

        /// <summary>
        /// Gets the weight of this edge.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets the Line instance of this edge that represents the visual edge.
        /// </summary>
        public Line Line { get; }

        /// <summary>
        /// Gets the label that represents the weight of this visual edge.
        /// </summary>
        public WeightLabel WeightLabel { get; private set; }

        /// <summary>
        /// Initializes an instance of the VisualEdge uisng speicifed infomation.
        /// </summary>
        /// <param name="v">An end point of this edge.</param>
        /// <param name="w">The other end point of this edge.</param>
        /// <param name="weight">The weight of this edge.</param>
        /// <param name="line">The visual line of this edge.</param>
        /// <param name="weightLabel">The visual label of this edge.</param>
        public VisualEdge(int v, int w, double weight, Line line, WeightLabel weightLabel)
        {
            // Check the range of the end point.
            if ((v < 0) || (w < 0))
                throw new ArgumentException("Vertex name must be a non-negative integer.");

            // Check the value of the weight.
            if (double.IsNaN(weight))
                throw new ArgumentException("Weight is NaN.");

            // Check whether the visual line instance is null.
            this.Line = line ?? throw new ArgumentException("The visual line is null.");

            // Initialize this edge using the speified info.
            this.v = v;
            this.w = w;
            this.Weight = weight;
            this.WeightLabel = weightLabel;
        }

        /// <summary>
        /// Returns either end point of this edge.
        /// </summary>
        /// <returns>Either end point of this edge.</returns>
        public int Either() => v;

        /// <summary>
        /// Returns the end point of this edge that is not the given one.
        /// </summary>
        /// <param name="vertex">The given end point.</param>
        /// <returns>The end point of this edge that is not the given one.</returns>
        public int Other(int vertex)
        {
            if (vertex == v)
                return w;
            else if (vertex == w)
                return v;
            else
                throw new ArgumentException("Illegal end-point.");
        }

        /// <summary>
        /// Compare the weight of 2 edges.
        /// </summary>
        /// <param name="that">The other VisualEdge instance.</param>
        /// <returns>
        /// 1 if the weight of this edge is larger than the other edge,
        /// -1 if the weight of this edge is less than the other edge,
        /// 0 if their weights are equal.
        /// </returns>
        public int CompareTo(VisualEdge that)
        {
            const double FloatingPointEpsilon = 1E-5;

            if (this.Weight - that.Weight < -FloatingPointEpsilon)
                return -1;
            else if (this.Weight - that.Weight > FloatingPointEpsilon)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Returns the string representation of this edge.
        /// </summary>
        /// <returns>the string representation of this edge.</returns>
        public override string ToString()
        {
            return string.Format("{0}-{1} {2:F2}", v, w, this.Weight);
        }
    }
}
