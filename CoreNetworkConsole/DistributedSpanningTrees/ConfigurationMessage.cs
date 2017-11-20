using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    /// <summary>
    /// The ConfigurationMessage class represents the spanning tree configuration message that stores the following information of a bridge: its ID, its root's ID, distance to its root.
    /// </summary>
    public class ConfigurationMessage
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
        /// Initializes an instance of BridgeInfo.
        /// </summary>
        /// <param name="selfId">The ID of this bridge.</param>
        /// <param name="rootId">The ID of this bridge's root.</param>
        /// <param name="distanceToRoot">The distance from this bridge to its root.</param>
        public ConfigurationMessage(int selfId, int rootId, int distanceToRoot)
        {
            SelfId = selfId;
            RootId = rootId;
            DistanceToRoot = distanceToRoot;
        }
    }
}
