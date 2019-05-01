using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    /// <summary>
    /// The EdgeWeightedPathsBase class represents a data type which is the base class for computing the shortest and the longest paths.
    /// </summary>
    public abstract class EdgeWeightedPathsBase
    {
        // distanceTo[v] = distance of source->v path.
        protected double[] distanceTo;

        // edgeTo[v] = last edge on source->v path.
        protected DirectedEdge[] edgeTo;

        /// <summary>
        /// Initializes the protected field for derived class to compute the shortest paths.
        /// </summary>
        /// <param name="G">The edge-weighted digraph.</param>
        /// <param name="source">The source vertex.</param>
        protected EdgeWeightedPathsBase(EdgeWeightedDigraph G, int source)
        {
            distanceTo = new double[G.V];
            edgeTo = new DirectedEdge[G.V];
        }

        /// <summary>
        /// Returns true if there is a path from the source vertex to vertex v, false otherwise.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>True if there is a path from the source vertex to vertex v, false otherwise.</returns>
        public virtual bool HasPathTo(int v) { return distanceTo[v] < double.PositiveInfinity; }

        /// <summary>
        /// Returns a shortest path from the source vertex to vertex v as an enumerator of edges, null if no such path.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>A shortest path from the source vertex to vertex v as an enumerator of edges, null if no such path.</returns>
        public virtual IEnumerable<DirectedEdge> PathTo(int v)
        {
            if (!HasPathTo(v))
                return null;

            Stack<DirectedEdge> path = new Stack<DirectedEdge>();
            int x = v;
            for (DirectedEdge e = edgeTo[x]; e != null; e = edgeTo[e.From()])
                path.Push(e);
            return path;
        }

        /// <summary>
        /// Returns the length of shortest path from the source vertex to the vertex v.
        /// </summary>
        /// <param name="v">The destination vertex.</param>
        /// <returns>The length of shortest path from the source vertex to the vertex v.</returns>
        public virtual double DistanceTo(int v) { return distanceTo[v]; }
    }
}