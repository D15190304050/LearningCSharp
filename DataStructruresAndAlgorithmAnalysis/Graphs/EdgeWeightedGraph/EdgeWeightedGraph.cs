using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedUndirectedGraph
{
    using BasicDataStructures;

    /// <summary>
    /// The EdgeWeightedGraph represents a edge-weighted graph of vertices named 0 through V-1,
    /// where undirected edge is type of Edge and has a double-value weight.
    /// </summary>
    public class EdgeWeightedGraph
    {
        // Adjacency lists of vertices
        private LinkedList<Edge>[] adjacent;

        /// <summary>
        /// The number of vertices of this edge weighted graph.
        /// </summary>
        public int V { get; private set; }

        /// <summary>
        /// The number of edges of this edge weighted graph.
        /// </summary>
        public int E { get; private set; }

        /// <summary>
        /// Initializes an empty edge-weighted graph with V vertices and 0 edges.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        public EdgeWeightedGraph(int V)
        {
            if (V < 0)
                throw new ArgumentException("Number of vertices must be non-negative.");

            this.V = V;
            E = 0;
            adjacent = new LinkedList<Edge>[V];
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<Edge>();
        }

        /// <summary>
        /// Initializes a random edge-weighted graph with V vertices and E edges.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        public EdgeWeightedGraph(int V, int E)
            : this(V)
        {
            if (E < 0)
                throw new ArgumentException("Number of edges must be non-negative.");

            for (int i = 0; i < E; i++)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                double weight = Math.Round(100 * StdRandom.Uniform() * 100.0) / 100.0;
                Edge e = new Edge(v, w, weight);
                AddEdge(e);
            }
        }

        /// <summary>
        /// Initializes an edge-weighted graph from a file.
        /// The format is number of vertices V,
        /// followed by the number of edges E,
        /// followed by E pair of vertices and edge weights,
        /// with each entry separated by white-space.
        /// </summary>
        /// <param name="fullFileName">The file which stores the edge-weighted graph.</param>
        public EdgeWeightedGraph(string fullFileName)
        {
            // Read all numbers separared by white-space as strings into an array.
            string text = System.IO.File.ReadAllText(fullFileName);
            string[] numberString = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // Get V and E.
            V = int.Parse(numberString[0]);
            int E = int.Parse(numberString[1]);
            if (V < 0)
                throw new ArgumentException("The number of vertices must be non-negative.");
            if (E < 0)
                throw new ArgumentException("The number of vertices must be non-negative.");

            // Initializes an emtpy edge-weighted graph.
            adjacent = new LinkedList<Edge>[V];
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<Edge>();

            // Read edges from index 2 in the numberString[].
            int numberIndex = 2;

            // Read edges and add them to this edge-weighted grpah.
            for (int i = 0; i < E; i++)
            {
                int v = int.Parse(numberString[numberIndex++]);
                int w = int.Parse(numberString[numberIndex++]);
                ValidateVertex(v);
                ValidateVertex(w);
                double weight = double.Parse(numberString[numberIndex++]);
                Edge e = new Edge(v, w, weight);
                AddEdge(e);
            }
        }

        /// <summary>
        /// Initializes a new edge-weighted graph that is a deep copy of G.
        /// </summary>
        /// <param name="G">The edge-weighted graph to copy.</param>
        public EdgeWeightedGraph(EdgeWeightedGraph G)
            : this(G.V)
        {
            E = G.E;

            for (int v = 0; v < V; v++)
            {
                // Reverse so that adjacency lists is in same order as origin.
                Stack<Edge> reverse = new Stack<Edge>();
                foreach (Edge e in G.adjacent[v])
                    reverse.Push(e);
                foreach (Edge e in reverse)
                    adjacent[v].AddFirst(e);
            }
        }

        /// <summary>
        /// Throw an ArugmentOutOfRangeException unless 0 &lt;= v &lt; V.
        /// </summary>
        /// <param name="v"></param>
        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentOutOfRangeException(string.Format("Vertex {0} is not between [0,{1}].", v, (V - 1).ToString()));
        }

        /// <summary>
        /// Adds the undirected edge to this edge-weighted graph.
        /// </summary>
        /// <param name="e">The edge to add to this edge-weighted graph.</param>
        public void AddEdge(Edge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            ValidateVertex(v);
            ValidateVertex(w);
            adjacent[v].AddFirst(e);
            adjacent[w].AddFirst(e);
            E++;
        }

        /// <summary>
        /// Returns the edges incident on vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The edges incident on vertex v.</returns>
        public IEnumerable<Edge> Adjacent(int v) { return adjacent[v]; }

        /// <summary>
        /// Returns the degree of vertex v.
        /// </summary>
        /// <param name="v">The vertex</param>
        /// <returns>The degree of vertex v.</returns>
        public int Degree(int v) { return adjacent[v].Size; }

        /// <summary>
        /// Returns all edges in this edge-weighted graph, as an enumerator.
        /// </summary>
        /// <returns>All edges in this edge-weighted graph, as an enumerator.</returns>
        public IEnumerable<Edge> Edges()
        {
            LinkedList<Edge> edges = new LinkedList<Edge>();
            for (int v = 0; v < V; v++)
            {
                int selfLoops = 0;
                foreach (Edge e in adjacent[v])
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
        /// Returns a string representation of the edge-weighted graph.
        /// </summary>
        /// <returns>The number of vertices V, followed by the number of edges E, followed by the V adjacency lists of edges.</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(V +  " vertices  " + E + " edges\n");
            for (int v = 0; v < V; v++)
            {
                s.Append(V + ": ");
                foreach (Edge e in adjacent[v])
                    s.Append(e + " ");
                s.Append('\n');
            }
            return s.ToString();
        }
    }
}