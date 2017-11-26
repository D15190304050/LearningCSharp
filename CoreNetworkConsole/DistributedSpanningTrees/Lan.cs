using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class Lan
    {
        public ConcurrentQueue<ConfigurationMessage> MessageQueue { get; set; }

        public int Id { get; private set; }

        public string Name { get; private set; }

        private LinkedList<Bridge> connections;

        public Lan(int id, string name = "")
        {
            this.Id = id;
            this.Name = name;
            connections = new LinkedList<Bridge>();
            this.MessageQueue = new ConcurrentQueue<ConfigurationMessage>();
        }

        public void AddConnection(Bridge bridge)
        {
            connections.AddLast(bridge);
        }

        public void Transmit()
        {
            while (MessageQueue.TryDequeue(out ConfigurationMessage message))
            {
                int senderId = message.SelfId;
                foreach (Bridge bridge in connections)
                {
                    if (bridge.Id != senderId)
                        bridge.MessageQueue.TryAdd(message, this);
                }
            }

            // Clear the message queue after processing every message in it.
            MessageQueue.Clear();
        }

        public override string ToString()
        {
            List<int> connectedBridges = new List<int>();
            foreach (Bridge bridge in connections)
            {
                if (bridge.ConnectedTo(this))
                    connectedBridges.Add(bridge.Id);
            }
            int designateBridgeId = connectedBridges[0];
            for (int i = 1; i < connectedBridges.Count; i++)
            {
                if (designateBridgeId > connectedBridges[i])
                    designateBridgeId = connectedBridges[i];
            }

            return string.Format($"The designate bridge for LAN {this.Id} is Bridge {designateBridgeId}");
        }
    }
}
