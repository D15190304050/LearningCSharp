using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class BridgeInfo
    {
        public int SelfId { get; private set; }

        public int RootId { get; set; }

        public int DistanceToRoot { get; set; }

        public BridgeInfo(int selfId, int rootId, int distanceToRoot)
        {
            SelfId = selfId;
            RootId = rootId;
            DistanceToRoot = distanceToRoot;
        }
    }
}
