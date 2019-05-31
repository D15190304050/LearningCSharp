using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    using Collections;

    public class Graph
    {
        // Adjacency lists.
        private LinkedList<int>[] adjacent;

        /// <summary>
        /// Create a V-vertex graph with no edges.
        /// </summary>
        /// <param name="V"></param>
        public Graph(int V)
        {
            this.V = V;
            E = 0;

            // Create array of lists.
            adjacent = new LinkedList<int>[V];

            // Initialize all lists to empty.
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<int>();
        }

        /// <summary>
        /// Read a graph from a file.
        /// </summary>
        /// <param name="fullFileName"></param>
        public Graph(string fullFileName)
        {
            // Read complete content from file.
            string text = System.IO.File.ReadAllText(fullFileName);

            // Split content into individual strings.
            string[] numberString = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // Convert numberString into numbers.
            int[] numbers = new int[numberString.Length];
            for (int i = 0; i < numberString.Length; i++)
                numbers[i] = int.Parse(numberString[i]);

            // Get V and E from numbers[].
            V = numbers[0];
            // To distinguish from this.E and make it easy to add edge to this graph in the following procedure..
            int E = numbers[1];

            if (V < 0 || E < 0)
                throw new ArgumentOutOfRangeException("Number of vertex or edges must be non-negative.");

            // Create array of adjacency lists.
            adjacent = new LinkedList<int>[V];

            // Initialize all adjacency lists to empty.
            for (int v = 0; v < V; v++)
                adjacent[v] = new LinkedList<int>();

            // Add edges from numbers[].
            for (int i = 1; i <= E; i++)
                AddEdge(numbers[2 * i], numbers[2 * i + 1]);
        }

        public Graph(Graph G)
            : this(G.V)
        {
            E = G.E;

            // Reverse so that adjacency list is in same order as original.
            for (int v = 0; v < V; v++)
            {
                Stack<int> reverse = new Stack<int>();
                foreach (int w in G.adjacent[v])
                    reverse.Push(w);
                foreach (int w in reverse)
                    adjacent[v].AddFirst(w);
            }
        }

        /// <summary>
        /// Number of vertices.
        /// </summary>
        public int V { get; private set; }

        /// <summary>
        /// Number of edges.
        /// </summary>
        public int E { get; private set; }

        /// <summary>
        /// Add edge v-w to this graph.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void AddEdge(int v, int w)
        {
            // Add w to v's list.
            adjacent[v].AddFirst(w);

            // Add v to w's list.
            adjacent[w].AddFirst(v);

            // Increase the edgd count.
            E++;
        }

        /// <summary>
        /// Vertices adjacent to v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<int> Adjacent(int v)
        {
            ValidateVertex(v);
            return adjacent[v];
        }

        /// <summary>
        /// Returns the degree of vertex.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Degree(int v)
        {
            ValidateVertex(v);
            return adjacent[v].Size;
        }

        /// <summary>
        /// Throw an ArgumentOutOfRangeException unless 0 &le; v &lt; V
        /// </summary>
        /// <param name="v"></param>
        private void ValidateVertex(int v)
        {
            if (v < 0 || v >= V)
                throw new ArgumentOutOfRangeException("Vertex " + v + " is not between 0 and " + (V - 1));
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Use StringBuilder to accelerate the processing.
            StringBuilder s = new StringBuilder();
            s.Append(V).Append(" vertices ").Append(E).Append(" edges\n");
            for (int v = 0; v < V; v++)
            {
                s.Append(v).Append(": ");
                foreach (int w in Adjacent(v))
                    s.Append(w).Append(" ");
                s.Append("\n");
            }

            return s.ToString();
        }
    }
}
