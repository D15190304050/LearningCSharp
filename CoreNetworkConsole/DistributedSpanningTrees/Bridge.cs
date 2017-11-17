using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class Bridge
    {
        private BridgeInfo selfInfo;

        public bool IsRoot { get { return selfInfo.DistanceToRoot == 0; } }

        public int NextHopToRoot { get; private set; }

        private LinkedList<BridgeSlim> adjacent;

        public IEnumerable<BridgeSlim> Adjacent { get { return adjacent; } }

        public Bridge(int selfId)
        {
            selfInfo = new BridgeInfo(selfId, selfId, 0);
            adjacent = new LinkedList<BridgeSlim>();
        }

        public void AddAdjacent(int bridgeId)
        {
            adjacent.AddLast(new BridgeSlim(bridgeId));
        }

        public void Update(ConcurrentQueue<BridgeInfo>[] messageQueues)
        {
            UpdateSelfInfo(messageQueues[selfInfo.SelfId]);
            SendConfigurationMessage(messageQueues);
        }

        private void UpdateSelfInfo(ConcurrentQueue<BridgeInfo> messageQueue)
        {
            foreach (BridgeInfo adjacentBridge in messageQueue)
            {
                int selfDistanceToRoot = selfInfo.DistanceToRoot;
                int adjacentDistanceToRoot = adjacentBridge.DistanceToRoot;
                if (selfDistanceToRoot > adjacentDistanceToRoot)
                {
                    selfInfo.DistanceToRoot = adjacentDistanceToRoot + 1;
                    selfInfo.RootId = adjacentBridge.RootId;
                }
                else if (selfDistanceToRoot == adjacentDistanceToRoot)
                {
                    if (selfInfo.RootId > adjacentBridge.RootId)
                        selfInfo.RootId = adjacentBridge.RootId;
                }
            }
        }

        private void SendConfigurationMessage(ConcurrentQueue<BridgeInfo>[] messageQueues)
        {
            if (this.IsRoot)
            {
                foreach (BridgeSlim adjacentBridge in adjacent)
                {
                    if (adjacentBridge.CanSend)
                        messageQueues[adjacentBridge.Id].Enqueue(selfInfo);
                }
            }
        }

        public override string ToString()
        {
            return string.Format($"Bridge ID = {selfInfo.SelfId}, Next hop to root = {NextHopToRoot}, Distance to root = {selfInfo.DistanceToRoot}");
        }
    }
}