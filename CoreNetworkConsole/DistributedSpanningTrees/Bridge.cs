using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    /// <summary>
    /// The Bridge class represents a bridge in an autonomous system.
    /// </summary>
    public class Bridge
    {
        /// <summary>
        /// Gets the ID of this bridge.
        /// </summary>
        public int SelfId { get; private set; }

        /// <summary>
        /// Gets or sets the ID of this bridge's root.
        /// </summary>
        public int RootId { get; set; }

        /// <summary>
        /// Gets or sets the distance from this bridge to its root.
        /// </summary>
        /// <remarks>
        /// Note that the distance here equals the number of edges on the path to reach the root from this bridge.
        /// </remarks>
        public int DistanceToRoot { get; set; }

        /// <summary>
        /// The designate bridge of this bridge.
        /// </summary>
        public int DesignateBridge { get; private set; }

        /// <summary>
        /// The adjacency list of this bridge.
        /// </summary>
        private LinkedList<int> adjacent;

        /// <summary>
        /// Gets the adjacency list of this bridge.
        /// </summary>
        public IEnumerable<int> Adjacent { get { return adjacent; } }

        /// <summary>
        /// Initializes an instance of Bridge with specified ID, note that the root ID will be set to the specified ID as well.
        /// </summary>
        /// <param name="selfId">The specified ID.</param>
        public Bridge(int selfId)
        {
            SelfId = selfId;
            RootId = selfId;
            DistanceToRoot = 0;
            adjacent = new LinkedList<int>();
            DesignateBridge = selfId;
        }

        /// <summary>
        /// Add an bridge to the adjacency list.
        /// </summary>
        /// <param name="bridgeId">The ID of the bridge to add.</param>
        public void AddAdjacent(int bridgeId)
        {
            adjacent.AddLast(bridgeId);
        }

        public void Update(ConcurrentQueue<ConfigurationMessage>[] messageQueues)
        {
            //if (NeedUpdate(messageQueues))
            {
                UpdateSelfInfo(messageQueues[SelfId]);
                SendConfigurationMessage(messageQueues);
            }
        }

        /// <summary>
        /// Updates the information of this bridge.
        /// </summary>
        /// <param name="messageQueue">The message queue that stores the message sent from adjacent bridges.</param>
        private void UpdateSelfInfo(ConcurrentQueue<ConfigurationMessage> messageQueue)
        {
            // Process every piece of message in the queue.
            while (messageQueue.TryDequeue(out ConfigurationMessage adjacentBridge))
            {
                // To make sure the algorithm will converge at the end, the bridge with smaller ID will always be set to the general root.
                // So, if the root ID of this bridge is larger than the root ID of the adjacent bridge, than an update is necessary.
                // Otherwise, if they have the same root ID, but go through the adjacent bridge will closer, than an update is necessary.
                // Note : don't forget to update the DistanceToRoot property.
                if (this.RootId > adjacentBridge.RootId)
                {
                    DesignateBridge = adjacentBridge.SelfId;
                    this.RootId = adjacentBridge.RootId;
                    this.DistanceToRoot = adjacentBridge.DistanceToRoot + 1;
                }
                else if (this.RootId == adjacentBridge.RootId)
                {
                    if (this.DistanceToRoot > adjacentBridge.DistanceToRoot + 1)
                    {
                        DesignateBridge = adjacentBridge.SelfId;
                        this.DistanceToRoot = adjacentBridge.DistanceToRoot + 1;
                    }
                    else if (this.DistanceToRoot == adjacentBridge.DistanceToRoot + 1)
                    {
                        if (this.DesignateBridge > adjacentBridge.SelfId)
                            this.DesignateBridge = adjacentBridge.SelfId;
                    }
                }
            }
        }

        /// <summary>
        /// Sends the configuration message of this bridge to every adjacent bridge.
        /// </summary>
        /// <param name="messageQueues">The message queues that stores the message that can be processed by other bridges.</param>
        private void SendConfigurationMessage(ConcurrentQueue<ConfigurationMessage>[] messageQueues)
        {
            foreach (int adjacentBridge in adjacent)
                messageQueues[adjacentBridge].Enqueue(new ConfigurationMessage(SelfId, RootId, DistanceToRoot));
        }

        /// <summary>
        /// Returns the string representation of this bridge, including the ID, Desigenate Bridge, and DistanceToRoot.
        /// </summary>
        /// <returns>The string representation of this bridge, including the ID, Desigenate Bridge, and DistanceToRoot.</returns>
        public override string ToString()
        {
            return string.Format($"Bridge ID = {SelfId}, Next hop to root = {DesignateBridge}, Distance to root = {DistanceToRoot}");
        }

        /// <summary>
        /// Restart the thread for computing spanning tree.
        /// </summary>
        public void Restart()
        {
            this.RootId = this.SelfId;
            this.DistanceToRoot = 0;
            DesignateBridge = this.SelfId;

            // Note : We don't need to refresh the adjacency list.
            // If you do that, the spanning tree algorithm will never converge.
        }
    }
}