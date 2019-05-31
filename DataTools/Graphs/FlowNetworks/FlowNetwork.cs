using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.FlowNetworks
{
    using Collections;

    /// <summary>
    /// The FlowNetwork class represents a capacitated network with vertices named 0 through V-1, where each directed edge is of type FlowEdge.
    /// </summary>
    public class FlowNetwork
    {
        /// <summary>
        /// Gets the number of vertices in this FlowNetwork.
        /// </summary>
        public int V { get; private set; }

        /// <summary>
        /// Gets the number of edges in this FlowNetwork.
        /// </summary>
        public int E { get; private set; }

        LinkedList<FlowEdge>[] adjacent;

        /// <summary>
        /// Initializes an empty flow network with V vertices and 0 edges.
        /// </summary>
        /// <param name="V">The number of vertices in this FlowNetwork.</param>
        public FlowNetwork(int V)
        {
            // Validate input arguments before initialization.
            if (V < 0)
                throw new ArgumentException("Number of vertices in a Graph must be non-negative.");

            // Initialize this FlowNetwork.
            this.V = V;
            this.E = 0;
            InitializeAdjacencyLists();
        }

        /// <summary>
        /// Initializes a random flow network with V vertices and E edges.
        /// </summary>
        /// <remarks>
        /// The capacities are integers between 0 and 99 and the flow values are 0.
        /// </remarks>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        public FlowNetwork(int V, int E) : this(V)
        {
            // Validate the number of edges.
            if (E < 0)
                throw new ArgumentException("Number of edges must by non-negative.");

            // Randomly create FlowEdges and add them to this FlowNetwork.
            for (int i = 0; i < E; i++)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                double capacity = StdRandom.Uniform(100);
                AddEdge(new FlowEdge(v, w, capacity));
            }
        }

        /// <summary>
        /// Initializes a flow network from a text file.
        /// </summary>
        /// <remarks>
        /// The format is the number of vertices V,
        /// followed by the number of edges E,
        /// followed by E pairs of vertices and edge capacities,
        /// with each entry separeted by white space.
        /// </remarks>
        /// <param name="fullFilePath">The file path of the file that stores the FlowNetwork to load.</param>
        public FlowNetwork(string filePath)
        {
            // Get all the contents in the text file separated by line separator.
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // Get the number of vertices and edges and then check their range.
            int V = int.Parse(lines[0]);
            int E = int.Parse(lines[1]);
            if (V < 0)
                throw new ArgumentException("Number of vertices in a Graph must be non-negative.");
            if (E < 0)
                throw new ArgumentException("Number of edges must be non-negative.");

            // Initialize the adjancy lists.
            this.V = V;
            InitializeAdjacencyLists();

            // Add edges to this FlowNetwork.
            for (int i = 2; i < lines.Length; i++)
            {
                // Separate the line to individual words by white space.
                string[] line = System.Text.RegularExpressions.Regex.Split(lines[i], "\\s+");

                // Get the tail and head vertices and then validate their range.
                int v = int.Parse(line[0]);
                int w = int.Parse(line[1]);
                ValidateVertex(v);
                ValidateVertex(w);

                // Get the capacity of next edge to add.
                double capacity = double.Parse(line[2]);

                // Add the new FlowEdge to this FlowNetwork.
                AddEdge(new FlowEdge(v, w, capacity));
            }
        }

        /// <summary>
        /// Initializes the adjancy lists of this FlowNetwork.
        /// </summary>
        private void InitializeAdjacencyLists()
        {
            adjacent = new LinkedList<FlowEdge>[V];
            for (int i = 0; i < V; i++)
                adjacent[i] = new LinkedList<FlowEdge>();
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
        /// Adds the FlowEdge e to this FlowNetwork.
        /// </summary>
        /// <param name="e">The edge to add.</param>
        public void AddEdge(FlowEdge e)
        {
            int v = e.From;
            int w = e.To;
            ValidateVertex(v);
            ValidateVertex(w);
            adjacent[v].AddFirst(e);
            adjacent[w].AddFirst(e);
            E++;
        }

        /// <summary>
        /// Returns the edges incident on vertex v (includes both edges pointing to and from v).
        /// </summary>
        /// <param name="v">The given vertex.</param>
        /// <returns>The edges incident on vertex v (includes both edges pointing to and from v).</returns>
        public IEnumerable<FlowEdge> Adjacent(int v)
        {
            ValidateVertex(v);
            return adjacent[v];
        }

        /// <summary>
        /// Returns a list of all edges, exclusive self loops.
        /// </summary>
        /// <returns>A list of all edges, exclusive self loops.</returns>
        public IEnumerable<FlowEdge> Edges()
        {
            LinkedList<FlowEdge> edges = new LinkedList<FlowEdge>();
            for (int v = 0; v < V; v++)
            {
                foreach (FlowEdge e in adjacent[v])
                {
                    if (e.To != v)
                        edges.AddFirst(e);
                }
            }

            return edges;
        }

        /// <summary>
        /// Returns the string representation of this FlowNetwork.
        /// </summary>
        /// <remarks>
        /// This methods takes time proportional to (V + E).
        /// </remarks>
        /// <returns>The string representation of this FlowNetwork.</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder(V + " vertices and " + E + " edges" + Environment.NewLine);
            for (int v = 0; v < V; v++)
            {
                s.Append(v + ": ");
                foreach (FlowEdge e in adjacent[v])
                {
                    if (e.To != v)
                        s.Append(e + " ");
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }
    }
}
