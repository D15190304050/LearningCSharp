using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    /// <summary>
    /// The VisualEdgeWeightedGraph class represents an VisualEdgeWeightedGraph of vertices named 0 through V - 1, where V represents the number of vertices in this graph.
    /// </summary>
    /// <remarks>
    /// The weighted edge in this graph is type of VisualEdge and has a double-valued weight.
    /// </remarks>
    public class VisualEdgeWeightedGraph
    {
        /// <summary>
        /// Adjacency lists of vertices.
        /// </summary>
        /// <remarks>
        /// Usually this should be an array of LinkedList&lt;T>.
        /// But here List is better than array because this change will make the data structure of this graph support the operation of adding a vertex.
        /// And SortedSet&lt;T> is better than LinkedList because this change will make the data structure of this graph able to remove an edge efficiently.
        /// </remarks>
        private List<SortedSet<VisualEdge>> adjacent;

        /// <summary>
        /// Gets the number of vertices of this VisualEdgeWeightedGraph.
        /// </summary>
        public int V { get; private set; }

        /// <summary>
        /// Gets the number of edges of this VisualEdgeWeightedGraph.
        /// </summary>
        public int E { get; private set; }

        /// <summary>
        /// Initializes an empty VisualEdgeWeightedGraph.
        /// </summary>
        public VisualEdgeWeightedGraph()
        {
            this.V = 0;
            this.E = 0;
            adjacent = new List<SortedSet<VisualEdge>>();
        }

        /// <summary>
        /// Throws an ArgumentException unless 0 &lt;= v &lt; V.
        /// </summary>
        /// <param name="v">The vertex to validate.</param>
        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentException(string.Format("Vertex {0} is not between [0,{1}].", v, (V - 1).ToString()));
        }

        /// <summary>
        /// Adds a VisualEdge to this VisualEdgeWeightedGraph.
        /// </summary>
        /// <param name="e">The edge to add to this VisualEdgeWeightedGraph.</param>
        /// <returns>True if adding this edge successfully, false otherwise.</returns>
        public bool AddEdge(VisualEdge e)
        {
            // Get end points of this edge.
            int v = e.Either();
            int w = e.Other(v);

            // Valiedate the range of the end points of this edge.
            ValidateVertex(v);
            ValidateVertex(w);

            // Return false if no such vertex.
            if ((adjacent[v] == null) || (adjacent[w] == null))
                return false;

            // Add it to this VisualEdgeWeightedGraph.
            adjacent[v].Add(e);
            adjacent[w].Add(e);

            // Update the edge counter.
            E++;

            // Add this edge successfully.
            return true;
        }

        /// <summary>
        /// Removes a VisualEdge from this VisualEdgeWeightedGraph.
        /// </summary>
        /// <param name="e">The edge to remove.</param>
        public void RemoveEdge(VisualEdge e)
        {
            // Get end points of this edge.
            int v = e.Either();
            int w = e.Other(v);

            // Remove this edge unless 2 end points of this edge are there in this VisualEdgeWeightedGraph.
            if ((adjacent[v] != null) && (adjacent[w] != null))
            {
                adjacent[v].Remove(e);
                adjacent[w].Remove(e);
                this.E--;
            }
        }

        /// <summary>
        /// Removes a vertex and all edges connected to this vertex of this VisualEdgeWeightedGraph if the vertex exists.
        /// </summary>
        /// <param name="v">The vertex to remove.</param>
        public void RemoveVertex(int v)
        {
            if (ContainsVertex(v))
            {
                IEnumerable<VisualEdge> adjacents = adjacent[v].ToArray();
                foreach (VisualEdge e in adjacents)
                    RemoveEdge(e);
                adjacent[v] = null;
            }
        }

        /// <summary>
        /// Add a vertex to this VisualEdgeWeightedGraph.
        /// </summary>
        public void AddVertex()
        {
            this.V++;
            adjacent.Add(new SortedSet<VisualEdge>());
        }

        /// <summary>
        /// Returns true if this VisualEdgeWeightedGraph contains the specified vertex, false otherwise.
        /// </summary>
        /// <param name="v">The specified vertex.</param>
        /// <returns>True if this VisualEdgeWeightedGraph contains the specified vertex, false otherwise.</returns>
        public bool ContainsVertex(int v)
        {
            if ((v < 0) || (v >= V))
                return false;
            else if (adjacent[v] == null)
                return false;
            return true;
        }

        /// <summary>
        /// Returns the degree of vertex v.
        /// </summary>
        /// <param name="v">The specified vertex.</param>
        /// <returns>The degree of vertex v.</returns>
        public int Degree(int v)
        {
            if (!ContainsVertex(v))
                throw new ArgumentException("There is no such vertex in this VisualEdgeWeightedGraph.");
            return adjacent[v].Count;
        }

        /// <summary>
        /// Returns the edges incident on vertex v, null if no such vertex..
        /// </summary>
        /// <param name="v">The specified vertex.</param>
        /// <returns>The edges incident on vertex v, null if no such vertex..</returns>
        public IEnumerable<VisualEdge> Adjacent(int v)
        {
            ValidateVertex(v);
            return adjacent[v];
        }

        /// <summary>
        /// Returns all edges in this VisualEdgeWeightedGraph.
        /// </summary>
        /// <returns>All edges in this VisualEdgeWeightedGraph.</returns>
        public IEnumerable<VisualEdge> Edges()
        {
            LinkedList<VisualEdge> edges = new LinkedList<VisualEdge>();
            for (int v = 0; v < V; v++)
            {
                if (adjacent[v] == null)
                    continue;

                int selfLoops = 0;
                foreach (VisualEdge e in adjacent[v])
                {
                    if (e.Other(v) > v)
                        edges.AddFirst(e);

                    // Only add one copy of self loop (self loop will be constructive).
                    else if (e.Other(v) == v)
                    {
                        if (selfLoops % 2 == 0)
                            edges.AddFirst(e);
                        selfLoops++;
                    }
                }
            }

            return edges;
        }

        /// <summary>
        /// Returns the string representation of this VisualEdgeWeightedGraph.
        /// </summary>
        /// <returns>The number of vertices, followed by the number of edges, followed by adjacency lists of this VisualEdgeWeightedGraph.</returns>
        public override string ToString()
        {
            int vertexCount = 0;
            StringBuilder s = new StringBuilder();
            for (int v = 0; v < V; v++)
            {
                if (adjacent[v] != null)
                {
                    vertexCount++;
                    s.Append(v + ": ");
                    foreach (VisualEdge e in adjacent[v])
                        s.Append(e + "; ");
                    s.Append(Environment.NewLine);
                }
            }

            return vertexCount + " vertices  " + E + " edges\n" + s.ToString();
        }

        /// <summary>
        /// Removes all vertices and edges from this VisualEdgeWeightedGraph.
        /// </summary>
        public void Clear()
        {
            this.V = 0;
            this.E = 0;
            adjacent.Clear();
        }
    }
}
