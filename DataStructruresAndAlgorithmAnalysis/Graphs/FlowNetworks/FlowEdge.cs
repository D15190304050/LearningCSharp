using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.FlowNetworks
{
    /// <summary>
    /// The FlowEdge class represents a capacitated edge with a flow in a FlowNetwork.
    /// </summary>
    public class FlowEdge
    {
        /// <summary>
        /// To deal with floating-point round-off errors.
        /// </summary>
        private const double FloatingPointEpsilon = 1E-10;

        /// <summary>
        /// The tail vertex.
        /// </summary>
        private int v;

        /// <summary>
        /// The head vertex.
        /// </summary>
        private int w;

        /// <summary>
        /// The capacity of this FlowEdge.
        /// </summary>
        private double capacity;

        /// <summary>
        /// The flow on this FlowEdge.
        /// </summary>
        private double flow;

        /// <summary>
        /// Gets the tail vertex of this edge.
        /// </summary>
        public int From { get { return v; } }

        /// <summary>
        /// Gets the head vertex of this edge.
        /// </summary>
        public int To { get { return w; } }

        /// <summary>
        /// Gets the capacity of this edge.
        /// </summary>
        public double Capacity { get { return capacity; } }

        /// <summary>
        /// Gets the flow on this edge.
        /// </summary>
        public double Flow { get { return flow; } }

        /// <summary>
        /// Initializes an edge from vertex v to vertex w with the given capacity and 0 flow.
        /// </summary>
        /// <param name="v">The tail vertex.</param>
        /// <param name="w">The head vertex.</param>
        /// <param name="capacity">The capacity of this edge.</param>
        public FlowEdge(int v, int w, double capacity)
        {
            // Validate input arguments before initialization.
            if (v < 0)
                throw new ArgumentException("Vertex index must be a non-negative integer.");
            if (w < 0)
                throw new ArgumentException("Vertex index must be a non-negative integer.");
            if (capacity < 0.0)
                throw new ArgumentException("Edge capacity must be non-negative.");

            // Initliaze this edge.
            this.v = v;
            this.w = w;
            this.capacity = capacity;
            this.flow = 0;
        }

        /// <summary>
        /// Initializes an edge from vertex v to vertex w with the given capacity and flow.
        /// </summary>
        /// <param name="v">The tail vertex.</param>
        /// <param name="w">The head vertex.</param>
        /// <param name="capacity">The capacity of this edge.</param>
        /// <param name="flow">The flow on this edge.</param>
        public FlowEdge(int v, int w, double capacity, double flow)
        {
            // Validate input arguments before initialization.
            if (v < 0)
                throw new ArgumentException("Vertex index must be a non-negative integer.");
            if (w < 0)
                throw new ArgumentException("Vertex index must be a non-negative integer.");
            if (capacity < 0.0)
                throw new ArgumentException("Edge capacity must be non-negative.");
            if (flow > capacity)
                throw new ArgumentException("Flow exceeds capacity.");
            if (flow < 0.0)
                throw new ArgumentException("Flow must be non-negative.");

            // Initliaze this edge.
            this.v = v;
            this.w = w;
            this.capacity = capacity;
            this.flow = flow;
        }

        /// <summary>
        /// Returns the end-point of this edge that is different from the given vertex.
        /// (unless this edge represents a self-loop in which case it returns the same vertex).
        /// </summary>
        /// <param name="vertex">One end-point of this edge.</param>
        /// <returns>The end-point of this edge that is different from the given vertex. (unless this edge represents a self-loop in which case it returns the same vertex).</returns>
        public int Other(int vertex)
        {
            if (vertex == v)
                return w;
            else if (vertex == w)
                return v;
            else
                throw new ArgumentException("Invalid end-point.");
        }

        /// <summary>
        /// Returns the residual capacity of this edge in the direction to the given vertex.
        /// </summary>
        /// <remarks>
        /// If vertex is the tail vertex (backward edge), the residual capacity equals flow;
        /// if vertex is the head vertex (forward edge), the residual capacity equals capacity - flow.
        /// </remarks>
        /// <param name="vertex">One end-point of this edge.</param>
        /// <returns>The residual capacity of this edge in the direction to the given vertex.</returns>
        public double ResidualCapacityTo(int vertex)
        {
            if (vertex == v)
                return flow;
            else if (vertex == w)
                return capacity - flow;
            else
                throw new ArgumentException("Invalid end-point.");
        }

        /// <summary>
        /// Increases the flow on this edge in the direction to the given vertex.
        /// </summary>
        /// <remarks>
        /// If vertex is the tail vertex, this decreases the flow on this edge by delta;
        /// if vertex is the head vertex, this increases the flow on this edge by delta.
        /// </remarks>
        /// <param name="vertex">One end-point of this edge.</param>
        /// <param name="delta">Amount by which to increase flow.</param>
        public void AddResidualFlowTo(int vertex, double delta)
        {
            if (delta < 0.0)
                throw new ArgumentException("Delta must be non-negative.");

            // vertex == v for backward edge, vertex == w for forward edge.
            if (vertex == v)
                flow -= delta;
            else if (vertex == w)
                flow += delta;
            else
                throw new ArgumentException("Invalid end-point.");

            // Round flow to 0 or capacity if within floating-point precision.
            if (Math.Abs(flow) <= FloatingPointEpsilon)
                flow = 0;
            if (Math.Abs(flow - capacity) <= FloatingPointEpsilon)
                flow = capacity;

            if (flow < 0.0)
                throw new ArgumentException("Flow is negative.");
            if (flow > capacity)
                throw new ArgumentException("Flow exceeds capacity.");
        }

        /// <summary>
        /// Returns the string representation of this edge.
        /// </summary>
        /// <returns>The string representation of this edge.</returns>
        public override string ToString()
        {
            return v + "->" + w + " " + flow + "/" + capacity;
        }
    }
}
