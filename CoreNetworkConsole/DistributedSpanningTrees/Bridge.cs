using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class Bridge
    {
        public BridgeInfo SelfInfo { get; private set; }

        public int Id { get { return SelfInfo.SelfId; } }

        public bool IsRoot { get { return SelfInfo.DistanceToRoot == 0; } }

        public int DesignateBridge { get; private set; }

        private LinkedList<BridgeSlim> adjacent;

        public IEnumerable<BridgeSlim> Adjacent { get { return adjacent; } }

        public Bridge(int selfId)
        {
            SelfInfo = new BridgeInfo(selfId, selfId, 0);
            adjacent = new LinkedList<BridgeSlim>();
            DesignateBridge = SelfInfo.SelfId;
        }

        public void AddAdjacent(int bridgeId)
        {
            adjacent.AddLast(new BridgeSlim(bridgeId));
        }

        public void Update(ConcurrentQueue<BridgeInfo>[] messageQueues)
        {
            UpdateSelfInfo(messageQueues[SelfInfo.SelfId]);
            SendConfigurationMessage(messageQueues);
        }

        private void UpdateSelfInfo(ConcurrentQueue<BridgeInfo> messageQueue)
        {
            foreach (BridgeInfo adjacentBridge in messageQueue)
            {
                int selfDistanceToRoot = SelfInfo.DistanceToRoot;
                int adjacentDistanceToRoot = adjacentBridge.DistanceToRoot;
                if (selfDistanceToRoot > adjacentDistanceToRoot)
                {
                    SelfInfo.DistanceToRoot = adjacentDistanceToRoot + 1;
                    SelfInfo.RootId = adjacentBridge.RootId;
                    DesignateBridge = adjacentBridge.SelfId;
                }
                else if (selfDistanceToRoot == adjacentDistanceToRoot)
                {
                    if (SelfInfo.RootId > adjacentBridge.RootId)
                    {
                        SelfInfo.RootId = adjacentBridge.RootId;
                        DesignateBridge = adjacentBridge.SelfId;
                    }
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
                        messageQueues[adjacentBridge.Id].Enqueue(SelfInfo);
                }
            }
        }

        public override string ToString()
        {
            return string.Format($"Bridge ID = {SelfInfo.SelfId}, Next hop to root = {DesignateBridge}, Distance to root = {SelfInfo.DistanceToRoot}");
        }
    }
}