using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class Lan
    {
        public int Id { get; private set; }

        public Bridge2 DesignateBridge { get; set; }

        private LinkedList<LanBridgeConnection> connections;

        public IEnumerable<LanBridgeConnection> Connections { get { return connections; } }

        private LinkedList<Socket> sockets;

        private Socket lanSocket;

        public Lan(int id)
        {
            this.Id = id;
            connections = new LinkedList<LanBridgeConnection>();

            sockets = new LinkedList<Socket>();
        }

        public void AddConnection(LanBridgeConnection conn)
        {
            if (conn.Lan.Id != this.Id)
                throw new ArgumentException("This connection is not about this LAN.");

            connections.AddLast(conn);
        }

        private void SetPort(int portNumber)
        {
            IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            lanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            lanSocket.Bind(new IPEndPoint(ip, portNumber));
            lanSocket.Listen(10);
        }

        private void ListenConnection()
        {
            while (lanSocket.Connected)
            {
                Socket nextSocket = lanSocket.Accept();

            }
        }

        //public void AddConnection

        public void Transmit(ConfigurationMessage message)
        {

        }
    }
}
