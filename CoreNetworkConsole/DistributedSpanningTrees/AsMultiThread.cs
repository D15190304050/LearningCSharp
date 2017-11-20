using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    /// <summary>
    /// The AsStandalone class represents the automonous system that runs the multi-thread spanning tree algorithm.
    /// </summary>
    public class AsMultiThread
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
        /// The BridgeUpdater class provides the method to update a bridge. This method is designed for the multi-thread spanning tree algorithm.
        /// </summary>
        private class BridgeUpdater
        {
            /// <summary>
            /// The message queues for every single bridge.
            /// </summary>
            private ConcurrentQueue<ConfigurationMessage>[] messageQueues;

            /// <summary>
            /// The bridge to update.
            /// </summary>
            public Bridge Bridge { get; set; }

            /// <summary>
            /// A delegate returns a boolean value and has no parameters whose return value indicates whether the algorithm is converged or not.
            /// </summary>
            private Func<bool> IsConverged;

            /// <summary>
            /// Initializes an instance of BridgeUpdated using specified information.
            /// </summary>
            /// <param name="bridge">The bridge to update.</param>
            /// <param name="messageQueues">The message queues for every single bridge.</param>
            /// <param name="isConverged">A delegate returns a boolean value and has no parameters whose return value indicates whether the algorithm is converged or not.</param>
            public BridgeUpdater(Bridge bridge, ConcurrentQueue<ConfigurationMessage>[] messageQueues, Func<bool> isConverged)
            {
                this.Bridge = bridge;
                this.messageQueues = messageQueues;
                this.IsConverged = isConverged;
            }

            /// <summary>
            /// Updates the bridge.
            /// </summary>
            public void Update()
            {
                // Run the multi-thread spanning tree algorithm until converge.
                while (true)
                {
                    // End the update operation if the algorithm converges.
                    if (IsConverged())
                        break;

                    // Try to access the message queues, lock the message queues and update this bridge if there is no lock.
                    if (Monitor.TryEnter(messageQueues))
                    {
                        // Update the bridge.
                        Bridge.Update(messageQueues);

                        // Unlock the message queues.
                        Monitor.Exit(messageQueues);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes an empty automonous system with specified number of bridges.
        /// </summary>
        /// <param name="V">The number of bridges in this AS.</param>
        public AsMultiThread(int V)
        {
            Initialize(V);
        }


        /// <summary>
        /// Initializes an automonous system from a file.
        /// </summary>
        /// <param name="filePath">The path of the text file that stores the automonous system.</param>
        public AsMultiThread(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
                throw new ArgumentException("No such file.");

            string[] lines = File.ReadAllLines(filePath);
            this.V = int.Parse(lines[0]);
            int E = int.Parse(lines[1]);

            Initialize(this.V);

            for (int i = 0; i < E; i++)
            {
                string[] line = lines[i + 2].Split(' ');
                int v = int.Parse(line[0]);
                int w = int.Parse(line[1]);

                AddEdge(v, w);
            }
        }

        /// <summary>
        /// Initializes internal data structures of this AS.
        /// </summary>
        /// <param name="V">The number of bridges in this AS.</param>
        private void Initialize(int V)
        {
            this.V = V;
            this.E = 0;

            bridges = new Bridge[V];
            for (int v = 0; v < V; v++)
                bridges[v] = new Bridge(v);

            rootId = new int[V];
            for (int v = 0; v < V; v++)
                rootId[v] = v;

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
            ValidateVertex(v);
            ValidateVertex(w);

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
            ValidateVertex(v);
            return bridges[v].Adjacent;
        }

        /// <summary>
        /// Returns the string representation of this automonous system.
        /// </summary>
        /// <returns>the string representation of this automonous system.</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder($"{V} bridges and {E} edges." + Environment.NewLine);

            for (int v = 0; v < this.V; v++)
            {
                s.Append(v + " : ");
                foreach (int w in Adjacent(v))
                    s.Append(w + " ");
                s.Append(Environment.NewLine);
            }

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

            BridgeUpdater[] bridgeUpdaters = new BridgeUpdater[V];
            for (int v = 0; v < this.V; v++)
                bridgeUpdaters[v] = new BridgeUpdater(bridges[v], messageQueues, IsConverged);

            AutoResetEvent[] missions = new AutoResetEvent[V];

            Thread[] updaters = new Thread[V];
            for (int v = 0; v < this.V; v++)
            {
                int i = v;
                missions[i] = new AutoResetEvent(false);

                updaters[i] = new Thread(() => { bridgeUpdaters[i].Update(); missions[i].Set(); });
            }

            for (int v = 0; v < this.V; v++)
                updaters[v].Start();

            WaitHandle.WaitAll(missions);
        }

        /// <summary>
        /// Returns true if the spanning tree algorithm converges, otherwise, false.
        /// </summary>
        /// <returns>True if the spanning tree algorithm converges, otherwise, false.</returns>
        private bool IsConverged()
        {
            for (int v = 0; v < V; v++)
                rootId[v] = bridges[v].RootId;

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
            StringBuilder s = new StringBuilder();

            for (int v = 0; v < this.V; v++)
            {
                s.Append(bridges[v].ToString());
                s.Append(Environment.NewLine);
            }

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