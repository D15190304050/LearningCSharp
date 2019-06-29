using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using Collections;

    /// <summary>
    /// The Digraph class represents a directed graph of vertices name 0 through V-1.
    /// </summary>
    public class Digraph
    {
        /// <summary>
        /// Number of vertices in this digraph.
        /// </summary>
        public int V { get; private set; }

        /// <summary>
        /// Number of edges in this digraph.
        /// </summary>
        public int E { get; private set; }

        // Adjacent[v] = adjacency list for vertex v.
        private readonly LinkedList<int>[] adjacent;

        // indegree[v] = indegree of vertex v.
        private readonly int[] indegree;

        /// <summary>
        /// Initialize an empty digraph with V vertices.
        /// </summary>
        /// <param name="V"></param>
        public Digraph(int V)
        {
            if (V < 0)
                throw new ArgumentException("Number of vertices in a digraph must be non-negative.");

            this.V = V;
            E = 0;
            indegree = new int[V];
            adjacent = new LinkedList<int>[V];
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<int>();
        }

        /// <summary>
        /// Initialize a digraph from the specified file.
        /// </summary>
        /// <param name="fileName"></param>
        public Digraph(string fullFileName)
        {
            // Read complete content from file.
            string text = System.IO.File.ReadAllText(fullFileName);

            // Split content into individual strings.
            string[] numberString = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // Convert number string into numbers.
            int[] numbers = new int[numberString.Length];
            for (int i = 0; i < numberString.Length; i++)
                numbers[i] = int.Parse(numberString[i]);

            // Get V and E from numbers[].
            V = numbers[0];
            E = numbers[1];

            if (V < 0 || E < 0)
                throw new ArgumentOutOfRangeException("Number of vertices or edges must be non-negative.");

            // Create arrays of indegree and adjacency lists.
            indegree = new int[V];
            adjacent = new LinkedList<int>[V];

            // Initialize all adjacency lists to empty.
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<int>();

            // Add edges from numbers[].
            for (int e = 1; e <= E; e++)
                adjacent[numbers[2 * e]].AddFirst(numbers[2 * e + 1]);
        }

        /// <summary>
        /// Initialize a new digraph that is a deep copy of the specified digraph.
        /// </summary>
        /// <param name="G">The digraph to copy.</param>
        public Digraph(Digraph G)
            : this(G.V)
        {
            E = G.E;
            for (int v = 0; v < G.V; v++)
                indegree[v] = G.indegree[v];

            //  Reverse so that adjacency list is in the same order as original.
            for (int v = 0; v < G.V; v++)
            {
                Stack<int> reverse = new Stack<int>();
                foreach (int w in G.Adjacent(v))
                    reverse.Push(w);
                foreach (int w in reverse)
                    adjacent[v].AddFirst(w);
            }
        }

        /// <summary>
        /// Returns vertices adjacent from vertex v in this digraph.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>Vertices adjacent from vertex v in this digraph.</returns>
        public IEnumerable<int> Adjacent(int v)
        {
            ValidateVertex(v);
            return adjacent[v];
        }

        /// <summary>
        /// Throw an ArgumentOutOfRangeException unless 0 &lt;= v &lt;V.
        /// </summary>
        /// <param name="v">The vertex.</param>
        private void ValidateVertex(int v)
        {
            if (v < 0 || v >= V)
                throw new ArgumentOutOfRangeException(string.Format("Vertex {0} is not between 0 and {1}.", v, V));
        }

        /// <summary>
        /// Adds the directed edge v->w to this digraph.
        /// </summary>
        /// <param name="v">The source vertex.</param>
        /// <param name="w">The target vertex.</param>
        public void AddEdge(int v, int w)
        {
            ValidateVertex(v);
            ValidateVertex(w);
            adjacent[v].AddFirst(w);
            indegree[w]++;
            E++;
        }

        /// <summary>
        /// Returns the number of directed edges incident from vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The number of directed edges incident from vertex v.</returns>
        public int OutDegree(int v)
        {
            ValidateVertex(v);
            return adjacent[v].Size;
        }

        /// <summary>
        /// Returns the number of directed edges incident to vertex v.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The number of directed edges incident to vertex v.</returns>
        public int InDegree(int v)
        {
            ValidateVertex(v);
            return indegree[v];
        }

        /// <summary>
        /// Returns the reversion of this digraph.
        /// </summary>
        /// <returns>The reversion of this digraph.</returns>
        public Digraph Reverse()
        {
            Digraph reverse = new Digraph(V);
            for (int v = 0; v < V; v++)
            {
                foreach (int w in Adjacent(v))
                    reverse.AddEdge(w, v);
            }
            return reverse;
        }

        /// <summary>
        /// Returns a string representation of the digraph.
        /// </summary>
        /// <returns>The number of vertices V, followed by the number of edges E, followed by the V adjacency lists.</returns>
        public override string ToString()
        {
            StringBuilder digraph = new StringBuilder();
            digraph.Append(V + " vertices and " + E + " edges\n");
            for (int v = 0; v < V; v++)
            {
                digraph.Append(v + ": ");
                foreach (int w in Adjacent(v))
                    digraph.Append(w + " ");
                digraph.Append("\n");
            }
            return digraph.ToString();
        }
    }
}
