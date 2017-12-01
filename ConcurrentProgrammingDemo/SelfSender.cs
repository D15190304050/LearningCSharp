using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConcurrentProgrammingDemo
{
    public class SelfSender
    {
        private const int BufferSize = 1024;
        
        private byte[] messageToSend;

        private Socket senderSocket;
        private Socket receiverSocket;

        public SelfSender(int portNumber)
        {
            messageToSend = new byte[BufferSize];
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            senderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            senderSocket.Bind(new IPEndPoint(ip, portNumber));

        }
    }
}
