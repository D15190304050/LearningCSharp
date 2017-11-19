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

        public bool NeedUpdate(ConcurrentQueue<BridgeInfo>[] messageQueues)
        {
            ConcurrentQueue<BridgeInfo> inputMessage = messageQueues[SelfInfo.SelfId];

            //lock (messageQueues)
            if (!inputMessage.IsEmpty)
            {
                foreach (BridgeInfo adjacentBridge in inputMessage)
                {
                    if (adjacentBridge.RootId < SelfInfo.RootId)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
                return true;
        }

        public void Update(ConcurrentQueue<BridgeInfo>[] messageQueues)
        {
            //if (NeedUpdate(messageQueues))
            {
                UpdateSelfInfo(messageQueues[SelfInfo.SelfId]);
                SendConfigurationMessage(messageQueues);
            }
        }

        private void UpdateSelfInfo(ConcurrentQueue<BridgeInfo> messageQueue)
        {
            while (messageQueue.TryDequeue(out BridgeInfo adjacentBridge))
            {
                if (SelfInfo.RootId > adjacentBridge.RootId)
                {
                    DesignateBridge = adjacentBridge.SelfId;
                    SelfInfo.RootId = adjacentBridge.RootId;
                    SelfInfo.DistanceToRoot = adjacentBridge.DistanceToRoot + 1;
                }
                else if (SelfInfo.RootId == adjacentBridge.RootId)
                {
                    if (SelfInfo.DistanceToRoot > adjacentBridge.DistanceToRoot + 1)
                    {
                        DesignateBridge = adjacentBridge.SelfId;
                        SelfInfo.DistanceToRoot = adjacentBridge.DistanceToRoot + 1;
                    }
                }
            }
        }

        private void SendConfigurationMessage(ConcurrentQueue<BridgeInfo>[] messageQueues)
        {
            foreach (BridgeSlim adjacentBridge in adjacent)
            {
                if (adjacentBridge.CanSend)
                    messageQueues[adjacentBridge.Id].Enqueue(SelfInfo);
            }
        }

        public override string ToString()
        {
            return string.Format($"Bridge ID = {SelfInfo.SelfId}, Next hop to root = {DesignateBridge}, Distance to root = {SelfInfo.DistanceToRoot}");
        }

        public void Restart()
        {
            SelfInfo.RootId = SelfInfo.SelfId;
            SelfInfo.DistanceToRoot = 0;
            DesignateBridge = SelfInfo.SelfId;

            // Note : We don't need to refresh the adjacency list.
            // If you do that, the spanning tree algorithm will never converge.
        }
    }
}