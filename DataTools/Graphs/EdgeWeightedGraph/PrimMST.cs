using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedUndirectedGraph
{
    using Collections;
    using Sort;

    /// <summary>
    /// The PrimMST class represents a data type for computing a minimum spanning tree in an edge-weighted graph.
    /// </summary>
    public class PrimMST : MinSpanningTreeBase
    {
        // edgeTo[v] = shortest edge from tree vertex to non-tree vertex.
        private Edge[] edgeTo;

        // distanceTo[v] = weight of shotest such edge.
        private double[] distanceTo;

        // marked[v] = true if v on tree, false otherwise.
        private bool[] marked;

        // An index min pirority queue to select the next edge to add to the MST.
        IndexMinPQ<double> pq;

        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        public PrimMST(EdgeWeightedGraph G)
            : base()
        {
            edgeTo = new Edge[G.V];
            distanceTo = new double[G.V];
            marked = new bool[G.V];
            pq = new IndexMinPQ<double>(G.V);
            for (int v = 0; v < G.V; v++)
                distanceTo[v] = double.PositiveInfinity;

            // Run from each vertex to find minimum spanning tree or forest.
            for (int v = 0; v < G.V; v++)
            {
                if (!marked[v])
                    Prim(G, v);
            }

            // Compute the weight here.
            foreach (Edge e in Edges())
                Weight += e.Weight;
        }

        /// <summary>
        /// Run Prim's algorithm in graph G, starting from s.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        /// <param name="s">The starting vertex.</param>
        private void Prim(EdgeWeightedGraph G, int s)
        {
            distanceTo[s] = 0.0;
            pq.Add(s, distanceTo[s]);
            while (!pq.IsEmpty)
            {
                int v = pq.DeleteMin();
                Scan(G, v);
            }
        }

        /// <summary>
        /// Scan vertex v.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        /// <param name="v">The vertex.</param>
        private void Scan(EdgeWeightedGraph G, int v)
        {
            marked[v] = true;

            foreach (Edge e in G.Adjacent(v))
            {
                int w = e.Other(v);

                // v-w is obsolete edge.
                if (marked[w])
                    continue;

                if (e.Weight < distanceTo[w])
                {
                    distanceTo[w] = e.Weight;
                    edgeTo[w] = e;

                    if (pq.Contains(w))
                        pq.DecreaseKey(w, distanceTo[w]);
                    else
                        pq.Add(w, distanceTo[w]);
                }
            }
        }

        /// <summary>
        /// Returns the edges in a minimum spanning tree (or forest) as an IEnumerable of edges.
        /// </summary>
        /// <returns>The edges in a minimum spanning tree (or forest) as an IEnumerable of edges.</returns>
        public override IEnumerable<Edge> Edges()
        {
            // Short circuit。
            if (mst != null)
                return mst;

            mst = new Queue<Edge>();
            for (int v = 0; v < edgeTo.Length; v++)
            {
                Edge e = edgeTo[v];
                if (e != null)
                    mst.Enqueue(e);
            }
            return mst;
        }
    }
}