using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    using Collections;

    /// <summary>
    /// The BellmanFordShortestPaths class represents a data type for solving the single-source shortest paths problem in edge-weighted digraphs with no negative cycles.
    /// </summary>
    public class BellmanFordShortestPaths : ShortestPathBase
    {
        // onQueue[v] = true if v is currently on the queue, false otherwise.
        private bool[] onQueue;

        // Queue of vertices to relax.
        private Queue<int> queue;

        // Number of calls to relax().
        private int cost;

        // Negative cycle, null if no such cycle.
        private IEnumerable<DirectedEdge> cycle;

        /// <summary>
        /// Returns true if there is a negative cycle reachable from the source vertex, false otherwise.
        /// </summary>
        public bool HasNegativeCycle
        {
            get { return cycle != null; }
        }

        /// <summary>
        /// Computes a shortest paths tree from the source vertex to every other vertices in the edge-weighted digraph G.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        /// <param name="source">the source vertex.</param>
        public BellmanFordShortestPaths(EdgeWeightedDigraph G, int source)
            : base(G, source)
        {
            cost = 0;
            onQueue = new bool[G.V];
            cycle = null;

            // BellmanFord algorithm.
            queue = new Queue<int>();
            queue.Enqueue(source);
            onQueue[source] = true;
            while ((!queue.IsEmpty) && (!HasNegativeCycle))
            {
                int v = queue.Dequeue();
                onQueue[v] = false;
                Relax(G, v);
            }
        }

        /// <summary>
        /// Throw a NotSupportedException if the edge-weighted digraph has a negative-weighted directed cycle.
        /// </summary>
        private void HasNegativeCycleCheck()
        {
            if (HasNegativeCycle)
                throw new NotSupportedException("Negative cost cycle exists.");
        }

        /// <summary>
        /// Relax vertex v and put other end-points on queue if changed.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        /// <param name="v">The vertex to relax.</param>
        private void Relax(EdgeWeightedDigraph G, int v)
        {
            foreach (DirectedEdge e in G.Adjacent(v))
            {
                int w = e.To();
                if (distanceTo[w] > distanceTo[v] + e.Weight)
                {
                    distanceTo[w] = distanceTo[v] + e.Weight;
                    edgeTo[w] = e;
                    if (!onQueue[w])
                    {
                        queue.Enqueue(w);
                        onQueue[w] = true;
                    }
                }

                if (cost++ % G.V == 0)
                {
                    FindNegativeCycle();
                    if (HasNegativeCycle)
                        return;
                }
            }
        }

        /// <summary>
        /// Returns a negative cycle reachable from the source vertex, or null if there is no such cycle.
        /// </summary>
        /// <returns>A negative cycle reachable from the source vertex, or null if there is no such cycle.</returns>
        public IEnumerable<DirectedEdge> GetNegativeCycle() { return cycle; }

        /// <summary>
        /// Try to find a cycle in a predecessor graph.
        /// </summary>
        private void FindNegativeCycle()
        {
            int V = edgeTo.Length;
            EdgeWeightedDigraph spt = new EdgeWeightedDigraph(V);
            for (int v = 0; v < V; v++)
            {
                if (edgeTo[v] != null)
                    spt.AddEdge(edgeTo[v]);
            }

            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(spt);
            cycle = finder.GetCycle();
        }

        /// <summary>
        /// Returns the length of a shortest path from the source vertex to the destination vertex, double.PositiveInitinify if no such path.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>The length of a shortest path from the source vertex to the destination vertex, double.PositiveInitinify if no such path.</returns>
        public override double DistanceTo(int v)
        {
            HasNegativeCycleCheck();
            return distanceTo[v];
        }

        /// <summary>
        /// Returns a shortest path from the source vertex to the destination vertex.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>A shortest path from the source vertex to the destination vertex.</returns>
        public override IEnumerable<DirectedEdge> PathTo(int v)
        {
            HasNegativeCycleCheck();
            return base.PathTo(v);
        }
    }
}