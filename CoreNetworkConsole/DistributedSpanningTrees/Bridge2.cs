using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class Bridge2
    {
        public int Id { get; private set; }

        public int DistanceToRoot { get; set; }

        public int RootId { get; set; }

        public bool IsRoot { get { return Id == RootId; } }

        private LinkedList<LanBridgeConnection> connections;

        public IEnumerable<LanBridgeConnection> Connections { get { return connections; } }

        public Bridge2(int bridgeId)
        {
            this.Id = bridgeId;
            connections = new LinkedList<LanBridgeConnection>();
        }

        private void CutConnection(LanBridgeConnection conn)
        {
            connections.Remove(conn);

            if ((!IsRoot) && (connections.Count == 1))
                connections.Clear();
        }

        public void AddConnection(LanBridgeConnection conn)
        {
            if (conn.Bridge.Id != this.Id)
                throw new ArgumentException("The connection is not about this bridge.");

            connections.AddLast(conn);
        }

        public void SendConfigurationMessage()
        {
            ConfigurationMessage message = new ConfigurationMessage(this.Id, this.RootId, this.DistanceToRoot);

            foreach (LanBridgeConnection conn in connections)
                conn.MessageQueueToLan.Enqueue(message);
        }

        public void UpdateSelfInfo()
        {
            foreach (LanBridgeConnection conn in connections)
            {
                while (conn.MessageQueueToBridge.TryDequeue(out ConfigurationMessage message))
                {
                    if (message.RootId == 0) ;
                        //CutConnection(message.);
                }
            }
        }
    }
}
