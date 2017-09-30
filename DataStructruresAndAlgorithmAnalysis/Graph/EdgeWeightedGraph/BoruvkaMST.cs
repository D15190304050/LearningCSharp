using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.EdgeWeightedUndirectedGraph
{
    using BasicDataStructures;

    /// <summary>
    /// The BoruvkaMST class represents a data type for computing minimum spanning tree in an edge-weighted graph.
    /// </summary>
    public class BoruvkaMST : MinSpanningTreeBase
    {
        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="G">The edge-weighted graph.</param>
        public BoruvkaMST(EdgeWeightedGraph G)
        {
            mst = new Queue<Edge>();
            UnionFind uf = new UnionFind(G.V);

            // Repeat at most log V times or until we have (V-1) edges.
            for (int t = 1; (t < G.V) && (mst.Size < G.V - 1); t += t)
            {
                // For each tree in foeest, find closest edge.
                // If edge weights are equal, ties are broken in favor of first edge in G.Edges().
                Edge[] closest = new Edge[G.V];
                foreach (Edge e in G.Edges())
                {
                    int v = e.Either();
                    int w = e.Other(v);
                    int i = uf.Find(v);
                    int j = uf.Find(w);

                    // Same tree.
                    if (i == j)
                        continue;

                    if ((closest[i] == null) || Less(e, closest[i]))
                        closest[i] = e;
                    if ((closest[j] == null) || Less(e, closest[j]))
                        closest[j] = e;
                }

                // Add newly discovered edges to MST.
                for (int i = 0; i < G.V; i++)
                {
                    Edge e = closest[i];
                    if (e != null)
                    {
                        int v = e.Either();
                        int w = e.Other(v);

                        // Don't add the same edge twice.
                        if (!uf.Connected(v, w))
                        {
                            mst.Enqueue(e);
                            Weight += e.Weight;
                            uf.Union(v, w);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the weight of e1 is strictly less than that of e2, false otherwise.
        /// </summary>
        /// <param name="e1">An edge</param>
        /// <param name="e2">The other edge.</param>
        /// <returns>True if the weight of e1 is strictly less than that of e2, false otherwise.</returns>
        private static bool Less(Edge e1, Edge e2) { return e1.Weight < e2.Weight; }
    }
}