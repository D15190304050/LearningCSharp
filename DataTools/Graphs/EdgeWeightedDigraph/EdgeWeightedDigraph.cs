using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.EdgeWeightedDirectedGraph
{
    using Collections;

    /// <summary>
    /// The EdgeWeightedDigraph class represents an edge-weighted digraph of vertices named 0 through V-1,
    /// where each directed edge is a type of DirectedEdge and has a double-value weight.
    /// </summary>
    public class EdgeWeightedDigraph
    {
        /// <summary>
        /// Number of vertices in this digraph.
        /// </summary>
        public int V { get; private set; }

        /// <summary>
        /// Number of edges in this digraph.
        /// </summary>
        public int E { get; private set; }

        // Adjacency lists for vertices.
        private LinkedList<DirectedEdge>[] adjacent;

        // In degree of vertices.
        private int[] inDegree;

        /// <summary>
        /// Initializes an empty edge-weighted digrpah with V vertices and 0 edges.
        /// </summary>
        /// <param name="v">The number of vertices.</param>
        public EdgeWeightedDigraph(int V) { Initialize(V); }

        /// <summary>
        /// Returns a random edge-weighted digraph with V vertices and E edges.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        public EdgeWeightedDigraph(int V, int E)
            : this(V)
        {
            if (E < 0)
                throw new ArgumentException("Number of edges in a Digraph must be non-negative.");

            for (int i = 0; i < E; i++)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                double weight = StdRandom.Uniform(100) * 0.01;
                DirectedEdge e = new DirectedEdge(v, w, weight);
                AddEdge(e);
            }
        }

        /// <summary>
        /// Initializes an edge-weighted digraph from the specified file.
        /// The format is the number of vertices V,
        /// followed by the number of edges E,
        /// followed by E pairs of vertices and edge weights,
        /// with each entry separated by white-space.
        /// </summary>
        /// <param name="fullFileName"></param>
        public EdgeWeightedDigraph(string fullFileName)
        {
            // Read complete content from file.
            string text = System.IO.File.ReadAllText(fullFileName);

            // Split content into individual strings.
            string[] numberString = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // Get V and E.
            int V = int.Parse(numberString[0]);
            int E = int.Parse(numberString[1]);
            if (V < 0)
                throw new ArgumentException("The number of vertices must be non-negative.");
            if (E < 0)
                throw new ArgumentException("The number of edges must be non-negative.");

            // Initializes an empty edge-weighted digraph.
            Initialize(V);

            // Read edges from index 2 in the numberStrings[].
            int numberIndex = 2;

            // Read edges and add them into this edge-weighted digraph.
            for (int i = 0; i < E; i++)
            {
                int v = int.Parse(numberString[numberIndex++]);
                int w = int.Parse(numberString[numberIndex++]);
                ValidateVertex(v);
                ValidateVertex(w);
                double weight = double.Parse(numberString[numberIndex++]);
                DirectedEdge e = new DirectedEdge(v, w, weight);
                AddEdge(e);
            }
        }

        /// <summary>
        /// Initializes a new edge-weighted digraph that is a deep copy of G.
        /// </summary>
        /// <param name="G">The edge-weighted digraph to copy.</param>
        public EdgeWeightedDigraph(EdgeWeightedDigraph G)
            : this(G.V)
        {
            E = G.E;
            for (int v = 0; v < V; v++)
                inDegree[v] = G.inDegree[v];
            for (int v = 0; v < V; v++)
            {
                // Reverse so that adjacency list is in same order as original.
                Stack<DirectedEdge> reverse = new Stack<DirectedEdge>();
                foreach (DirectedEdge e in G.adjacent[v])
                    reverse.Push(e);
                foreach (DirectedEdge e in reverse)
                    AddEdge(e);
            }
        }

        /// <summary>
        /// Initializes an empty edge-weighted digraph with V vertices and 0 edges.
        /// This is a helper function for constructors.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        private void Initialize(int V)
        {
            if (V < 0)
                throw new ArgumentException("Number of vertices in a Digraph must be non-negative.");

            this.V = V;
            E = 0;
            inDegree = new int[V];
            adjacent = new LinkedList<DirectedEdge>[V];
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<DirectedEdge>();
        }

        /// <summary>
        /// Throw an ArgumentOutOfRangeException unless 0 &lt;= v &lt; V
        /// </summary>
        /// <param name="v">The vertex.</param>
        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentOutOfRangeException(string.Format("Vertex {0} is not between [0,{1}].", v, V - 1));
        }

        /// <summary>
        /// Adds the directed edge e to this edge-weighted digrpah.
        /// </summary>
        /// <param name="e">The edge.</param>
        public void AddEdge(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();
            ValidateVertex(v);
            ValidateVertex(w);
            adjacent[v].AddFirst(e);
            inDegree[w]++;
            E++;
        }

        /// <summary>
        /// Returns the directed edges incident from vertex v as an enumerator.
        /// </summary>
        /// <param name="v">The edge.</param>
        /// <returns>The directed edges incident from vertex v as an enumerator.</returns>
        public IEnumerable<DirectedEdge> Adjacent(int v)
        {
            ValidateVertex(v);
            return adjacent[v];
        }

        /// <summary>
        /// Returns the number of edges incident from vertex v.
        /// This is known as the out degree of vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The out-degree of vertex v.</returns>
        public int OutDegree(int v)
        {
            ValidateVertex(v);
            return adjacent[v].Size;
        }

        /// <summary>
        /// Returns the number of edges incident to vertex v.
        /// This is known as the in degree of vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The indegree of vertex v.</returns>
        public int InDegree(int v)
        {
            ValidateVertex(v);
            return inDegree[v];
        }

        /// <summary>
        /// Returns all directed edges in this directed digraph, as an enumerator.
        /// </summary>
        /// <returns>All directed edges in this directed digraph, as an enumerator.</returns>
        public IEnumerable<DirectedEdge> Edges()
        {
            LinkedList<DirectedEdge> list = new LinkedList<DirectedEdge>();
            for (int v = 0; v < V; v++)
            {
                foreach (DirectedEdge e in adjacent[v])
                    list.AddFirst(e);
            }
            return list;
        }

        /// <summary>
        /// Returns a string representation of this edge-weighted digraph.
        /// </summary>
        /// <returns>A string representation of this edge-weighted digraph.</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(V + " vertices and " + E + " edges\n");
            for (int v = 0; v < V; v++)
            {
                s.Append(v + ": ");
                foreach (DirectedEdge e in adjacent[v])
                    s.Append(e + " ");
                s.Append("\n");
            }
            return s.ToString();
        }
    }
}