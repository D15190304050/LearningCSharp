using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.IO;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    /// <summary>
    /// The AsStandalone class represents the automonous system that runs the standalone spanning tree algorithm.
    /// </summary>
    public class AsStandalone
    {
        /// <summary>
        /// The number of bridges in this AS.
        /// </summary>
        /// <remarks>
        /// V is a classic notation in discrete mathematics that represents the vertices in a graph.
        /// </remarks>
        public int V { get; private set; }

        /// <summary>
        /// The number of edges in this AS.
        /// </summary>
        /// <remarks>
        /// E is a classic notation in discrete mathematics that represents the vertices in a graph.
        /// </remarks>
        public int E { get; private set; }

        /// <summary>
        /// All the bridges in this AS.
        /// </summary>
        private Bridge[] bridges;

        /// <summary>
        /// The root ID of every bridge.
        /// </summary>
        private int[] rootId;

        /// <summary>
        /// The message queues for every single bridge.
        /// </summary>
        private ConcurrentQueue<ConfigurationMessage>[] messageQueues;

        /// <summary>
        /// Initializes an empty automonous system with specified number of bridges.
        /// </summary>
        /// <param name="V">The number of bridges in this AS.</param>
        public AsStandalone(int V)
        {
            Initialize(V);
        }

        /// <summary>
        /// Initializes an automonous system from a file.
        /// </summary>
        /// <param name="filePath">The path of the text file that stores the automonous system.</param>
        public AsStandalone(string filePath)
        {
            // Throw an exception if the file doesn't exist.
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
                throw new ArgumentException("No such file.");

            // Read all lines of the text file.
            string[] lines = File.ReadAllLines(filePath);

            // Get the number of vertices and edges.
            this.V = int.Parse(lines[0]);
            int E = int.Parse(lines[1]);

            // Initialize internal data structures.
            Initialize(this.V);

            // Process remaining lines one by one.
            for (int i = 0; i < E; i++)
            {
                // Split the line by space.
                string[] line = lines[i + 2].Split(' ');

                // Get 2 vertices of this edge.
                int v = int.Parse(line[0]);
                int w = int.Parse(line[1]);

                // Add this edge to this AS.
                AddEdge(v, w);
            }
        }

        /// <summary>
        /// Initializes internal data structures of this AS.
        /// </summary>
        /// <param name="V">The number of bridges in this AS.</param>
        private void Initialize(int V)
        {
            // Initialize the number of vertices and the number of edges.
            this.V = V;
            this.E = 0;

            // Initialize every bridge in this AS.
            bridges = new Bridge[V];
            for (int v = 0; v < V; v++)
                bridges[v] = new Bridge(v);

            // Initialize the root ID of every bridge.
            rootId = new int[V];
            for (int v = 0; v < V; v++)
                rootId[v] = v;

            // Initialize the message qeuues for every bridge.
            messageQueues = new ConcurrentQueue<ConfigurationMessage>[V];
            for (int v = 0; v < V; v++)
                messageQueues[v] = new ConcurrentQueue<ConfigurationMessage>();
        }

        /// <summary>
        /// Add an edge to this automonous system.
        /// </summary>
        /// <param name="v">A bridge of this automonous system.</param>
        /// <param name="w">The other bridge of this automonous system.</param>
        public void AddEdge(int v, int w)
        {
            // Validate the vertices before processing.
            ValidateVertex(v);
            ValidateVertex(w);

            // Add them to adjacency lists and update the edge counter.
            bridges[v].AddAdjacent(w);
            bridges[w].AddAdjacent(v);
            this.E++;
        }

        /// <summary>
        /// Returns the adjacency list of the specified bridge.
        /// </summary>
        /// <param name="v">The specified bridge.</param>
        /// <returns>The adjacency list of the specified bridge.</returns>
        public IEnumerable<int> Adjacent(int v)
        {
            // Validate the vertices before processing.
            ValidateVertex(v);

            // Return the adjacency list.
            return bridges[v].Adjacent;
        }

        /// <summary>
        /// Returns the string representation of this automonous system.
        /// </summary>
        /// <returns>the string representation of this automonous system.</returns>
        public override string ToString()
        {
            // Use StringBuilder to accelerate.
            StringBuilder s = new StringBuilder($"{V} bridges and {E} edges." + Environment.NewLine);

            // Add the adjacency list of every bridge to the StringBuilder.
            for (int v = 0; v < this.V; v++)
            {
                s.Append(v + " : ");
                foreach (int w in Adjacent(v))
                    s.Append(w + " ");
                s.Append(Environment.NewLine);
            }

            // Return the result.
            return s.ToString();
        }

        /// <summary>
        /// Computes the spanning tree of this automonous system.
        /// </summary>
        public void ComputeSpanningTree()
        {
            // Refresh the state information of every single bridge.
            for (int v = 0; v < this.V; v++)
                bridges[v].Restart();

            // Compute the spanning tree of this AS.
            // This algorithm will keep running until converge.
            while (!IsConverged())
            {
                // Update the information of every bridge.
                foreach (Bridge bridge in bridges)
                    bridge.Update(messageQueues);

                // Update the root ID of every bridge.
                for (int v = 0; v < V; v++)
                    rootId[v] = bridges[v].RootId;
            }
        }

        /// <summary>
        /// Returns true if the spanning tree algorithm converges, otherwise, false.
        /// </summary>
        /// <returns>True if the spanning tree algorithm converges, otherwise, false.</returns>
        private bool IsConverged()
        {
            int potentialRoot = rootId[0];
            for (int v = 1; v < V; v++)
            {
                if (potentialRoot != rootId[v])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the string representation of the spanning tree computed before.
        /// </summary>
        /// <returns>The string representation of the spanning tree computed before.</returns>
        public string GetSpanningTree()
        {
            // Use StringBuilder to accelerate.
            StringBuilder s = new StringBuilder();

            // Add the configuration message of every bridge to the StringBuilder.
            for (int v = 0; v < this.V; v++)
            {
                s.Append(bridges[v].ToString());
                s.Append(Environment.NewLine);
            }

            // Return the result.
            return s.ToString();
        }

        /// <summary>
        /// Validates the range of the vertex v, throws an exception if v is out of range.
        /// </summary>
        /// <param name="v">The vertex to validate.</param>
        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentException($"The name of the vertex must between 0 and {V - 1}.");
        }
    }
}