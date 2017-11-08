using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.FlowNetworks
{
    using BasicDataStructures;

    /// <summary>
    /// The FordFulkerson class represents a data type for computing a max-flow in a flow network.
    /// </summary>
    /// <remarks>
    /// This implementation uses the Ford-Fulkerson algorithm with the shortest augmenting path heuristic.
    /// </remarks>
    public class FordFulkerson
    {
        /// <summary>
        /// To deal with floating-point round-off errors.
        /// </summary>
        private const double FloatingPointEpsilon = 1E-10;

        /// <summary>
        /// The number of vertices in the given FlowNetwork.
        /// </summary>
        private int V;

        /// <summary>
        /// marked[v] == true iff s->v path in residual graph.
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// Current value of max flow.
        /// </summary>
        private FlowEdge[] edgeTo;

        /// <summary>
        /// Gets the value of the maximum flow.
        /// </summary>
        public double FlowValue { get; private set; }

        /// <summary>
        /// Computes a max-flow and a min-cut in the given flow network from vertex s to vertex t.
        /// </summary>
        /// <param name="G">The flow network.</param>
        /// <param name="s">The source vertex.</param>
        /// <param name="t">The sink vertex.</param>
        public FordFulkerson(FlowNetwork G, int s, int t)
        {
            // Get the number of vertices.
            V = G.V;

            // Validate the source and sink vertices.
            ValidateVertex(s);
            ValidateVertex(t);

            // Throw ArgumentException if the source vertex euqals the sink vertex.
            if (s == t)
                throw new ArgumentException("Source equals sink.");

            // Throw ArgumentException if the initial flow is in-feasible.
            if (!IsFeasible(G, s, t))
                throw new ArgumentException("Initial flow is infeasible.");

            // Set the initial flow value to 0.
            FlowValue = 0;

            // While there exists an augmenting path, use it to increase the flow.
            while (HasAugmentingPath(G, s, t))
            {
                // Compute bottle-neck capacity.
                double bottleNeck = double.PositiveInfinity;
                for (int v = t; v != s; v = edgeTo[v].Other(v))
                    bottleNeck = Math.Min(bottleNeck, edgeTo[v].ResidualCapacityTo(v));

                // Augment flow.
                for (int v = t; v != s; v = edgeTo[v].Other(v))
                    edgeTo[v].AddResidualFlowTo(v, bottleNeck);

                // Update the flow value.
                FlowValue += bottleNeck;
            }

            // Check optimality conditions.
            if (!Check(G, s, t))
                throw new ArgumentException("The result doesn't satisfy the optimality condition.");
        }

        /// <summary>
        /// Throw an ArgumentException unless 0 &lt;= v &lt; V;
        /// </summary>
        /// <param name="v">One vertex in this FlowNetwork.</param>
        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentException("Vertex " + v + " is not between 0 and " + (V - 1));
        }

        /// <summary>
        /// Returns true if the specified vertex is on the s side of the min-cut, false otherwise.
        /// </summary>
        /// <param name="v">A vertex.</param>
        /// <returns>True if the specified vertex is on the s side of the min-cut, false otherwise.</returns>
        public bool InCut(int v)
        {
            ValidateVertex(v);
            return marked[v];
        }

        /// <summary>
        /// Returns true if there is an augmenting path, false otherwise.
        /// </summary>
        /// <remarks>
        /// If there is an augmenting path, upon termination edgeTo[] will contain a parent-link representation of such a path.
        /// This implementation finds a shortest augmenting path (fewest number of edges), which performs well both in theory and practice.
        /// </remarks>
        /// <param name="G"></param>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns>True if there is an augmenting path, false otherwise.</returns>
        private bool HasAugmentingPath(FlowNetwork G, int s, int t)
        {
            edgeTo = new FlowEdge[G.V];
            marked = new bool[G.V];

            // Breadth-first search.
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            marked[s] = true;
            while ((!queue.IsEmpty) && (!marked[t]))
            {
                int v = queue.Dequeue();

                foreach (FlowEdge e in G.Adjacent(v))
                {
                    int w = e.Other(v);

                    // If residual capacity from v to w.
                    if (e.ResidualCapacityTo(w) > 0)
                    {
                        if (!marked[w])
                        {
                            edgeTo[w] = e;
                            marked[w] = true;
                            queue.Enqueue(w);
                        }
                    }
                }
            }

            // Is there an augmenting path.
            return marked[t];
        }

        /// <summary>
        /// Returns excess flow of FlowNetwork G at vertex v.
        /// </summary>
        /// <param name="G">The given flow network.</param>
        /// <param name="v">The given veretex.</param>
        /// <returns>Excess flow of FlowNetwork G at vertex v.</returns>
        private double ExcessFlow(FlowNetwork G, int v)
        {
            double excess = 0.0;
            foreach (FlowEdge e in G.Adjacent(v))
            {
                if (e.From == v)
                    excess -= e.Flow;
                else
                    excess += e.Flow;
            }
            return excess;
        }

        /// <summary>
        /// Returns true if the given flow network is feasible, false otherwise.
        /// </summary>
        /// <param name="G">The given flow network.</param>
        /// <param name="s">The soruce vertex.</param>
        /// <param name="t">The sink vertex.</param>
        /// <returns>True if the given flow network is feasible, false otherwise.</returns>
        private bool IsFeasible(FlowNetwork G, int s, int t)
        {
            // Check that capacity constraints are satisfied.
            for (int v = 0; v < G.V; v++)
            {
                foreach (FlowEdge e in G.Adjacent(v))
                {
                    if ((e.Flow < -FloatingPointEpsilon) || (e.Flow > e.Capacity + FloatingPointEpsilon))
                    {
                        Console.Error.WriteLine("Edge does not satisfy capacity constraints: " + e);
                        return false;
                    }
                }
            }

            // Check that net flow into a vertex equals 0, except at source and sink.
            if (Math.Abs(FlowValue + ExcessFlow(G, s)) > FloatingPointEpsilon)
            {
                Console.Error.WriteLine("Excess at source = " + ExcessFlow(G, s));
                return false;
            }
            if (Math.Abs(FlowValue - ExcessFlow(G, t)) > FloatingPointEpsilon)
            {
                Console.Error.WriteLine("Excess at sink = " + ExcessFlow(G, t));
                return false;
            }

            for (int v = 0; v < G.V; v++)
            {
                if ((v == s) || (v == t))
                    continue;
                else if (Math.Abs(ExcessFlow(G, v)) > FloatingPointEpsilon)
                {
                    Console.Error.WriteLine("Net flow out of " + v + " doesn't equal to 0.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check optimality conditions.
        /// </summary>
        /// <param name="G">The given flow network.</param>
        /// <param name="s">The given source.</param>
        /// <param name="t">The given sink.</param>
        /// <returns>True if the result satisfy the optimality conditions, false otherwise.</returns>
        private bool Check(FlowNetwork G, int s, int t)
        {
            // Check the flow that is feasible.
            if (!IsFeasible(G, s, t))
            {
                Console.Error.WriteLine("Flow is infeasible.");
                return false;
            }

            // Check that s is on the source side of min cut and that t is not on source side.
            if (!InCut(s))
            {
                Console.Error.WriteLine("Source " + s + " is not on source side of min cut.");
                return false;
            }
            if (InCut(t))
            {
                Console.Error.WriteLine("Sink " + t + " is on source side of min cut.");
                return false;
            }

            // Check that value of min cut = value of max flow.
            double minCutValue = 0;
            for (int v = 0; v < G.V; v++)
            {
                foreach (FlowEdge e in G.Adjacent(v))
                {
                    if ((v == e.From) &&
                        (InCut(e.From)) &&
                        (!InCut(e.To)))
                        minCutValue += e.Capacity;
                }
            }

            if (Math.Abs(FlowValue - minCutValue) > FloatingPointEpsilon)
            {
                Console.Error.WriteLine("Max flow value = " + FlowValue + ", min cut value = " + minCutValue);
                return false;
            }

            return true;
        }
    }
}
