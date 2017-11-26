using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class Bridge
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public int RootId { get; private set; }
        public bool IsRoot { get { return Id == RootId; } }
        public int DistanceToRoot { get; private set; }

        private int nextHopToRoot;

        public ConcurrentDictionary<ConfigurationMessage, Lan> MessageQueue { get; set; }

        private Dictionary<Lan, bool> connected;

        public Bridge(int id, string name = "")
        {
            this.Id = id;
            this.Name = name;
            this.RootId = id;
            this.MessageQueue = new ConcurrentDictionary<ConfigurationMessage, Lan>();
            connected = new Dictionary<Lan, bool>();
            nextHopToRoot = id;
        }

        public void AddConnection(Lan lan)
        {
            connected.Add(lan, true);
        }

        public bool ConnectedTo(Lan lan)
        {
            return connected[lan];
        }

        private void UpdateSelfInfo()
        {
            foreach (KeyValuePair<ConfigurationMessage, Lan> messageInfo in MessageQueue)
            {
                ConfigurationMessage message = messageInfo.Key;
                Lan lan = messageInfo.Value;
                if (message.RootId < this.RootId)
                {
                    this.RootId = message.RootId;
                    this.DistanceToRoot = message.DistanceToRoot + 1;
                    nextHopToRoot = message.SelfId;
                }
                else if ((message.RootId == this.RootId) && (nextHopToRoot != message.SelfId))
                {
                    if (message.DistanceToRoot < this.DistanceToRoot)
                    {
                        this.DistanceToRoot = message.DistanceToRoot + 1;
                        connected[lan] = false;
                    }
                    else if (message.DistanceToRoot == this.DistanceToRoot)
                    {
                        if (this.Id > message.SelfId)
                            connected[lan] = false;
                    }
                }
            }

            // Clear the message queue after processing every message in it.
            MessageQueue.Clear();

            LinkedList<Lan> connectedLans = new LinkedList<Lan>();
            foreach (KeyValuePair<Lan, bool> connection in connected)
            {
                if (connection.Value)
                    connectedLans.AddLast(connection.Key);
            }
            if (connectedLans.Count == 1)
                connected[connectedLans.First.Value] = false;
        }

        private void SendConfigurationInfo()
        {
            ConfigurationMessage message = new ConfigurationMessage(this.Id, this.RootId, this.DistanceToRoot);
            foreach (Lan lan in connected.Keys)
            {
                if (connected[lan])
                    lan.MessageQueue.Enqueue(message);
            }
        }

        public void Update()
        {
            UpdateSelfInfo();
            SendConfigurationInfo();
        }

        public void Clear()
        {
            this.RootId = this.Id;
            this.DistanceToRoot = 0;
            LinkedList<Lan> connectedLans = new LinkedList<Lan>(connected.Keys);
            foreach (Lan lan in connectedLans)
                connected[lan] = true;
        }

        public override string ToString()
        {
            return string.Format($"The root ID for Bridge {this.Id} is {this.RootId}, distance to root = {this.DistanceToRoot}");
        }

        public string GetConnectionInfo()
        {
            StringBuilder connectionInfo = new StringBuilder();
            foreach (Lan lan in connected.Keys)
                connectionInfo.Append($"LAN {lan.Id} -- Bridge {this.Id}" + Environment.NewLine);
            return connectionInfo.ToString();
        }
    }
}