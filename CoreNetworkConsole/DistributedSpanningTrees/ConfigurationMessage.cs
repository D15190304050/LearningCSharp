using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class ConfigurationMessage
    {
        public int SelfId { get; private set; }
        public int RootId { get; private set; }
        public int DistanceToRoot { get; private set; }

        public ConfigurationMessage(int selfId, int rootId, int distanceToRoot)
        {
            this.SelfId = selfId;
            this.RootId = rootId;
            this.DistanceToRoot = distanceToRoot;
        }
    }
}
