using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CoreNetworkConsole
{
    public class NatHoleClient
    {
        private byte[] result = new byte[1024];
        private Socket clientSocket;
        private bool started;
        private IPAddress ip;
        private int port;
        private ManualResetEvent connected;
        private Socket peer = null;

        private void ListenServer()
        {
            while (true)
            {
                result = new byte[1024];
                int contentLength = clientSocket.Receive(result);
                string content = Encoding.ASCII.GetString(result, 0, contentLength);
                Console.WriteLine(content);

                if ((started))
                {
                    if (content.IndexOf("Listen") > -1)
                    {
                        WaitForConnection();
                        break;
                    }
                    else
                    {
                        BuildConnection(content);
                        break;
                    }
                }

                if (content.IndexOf("say hello") > -1)
                {
                    started = true;
                    continue;
                }
            }

            for (; ; )
            {
                if (!peer.Connected)
                    continue;
                result = new byte[1024];
                int contentLength = peer.Receive(result);
                string content = Encoding.ASCII.GetString(result, 0, contentLength);
                Console.WriteLine(content);
            }
        }

        private void BuildConnection(string content)
        {
            string[] remoteClient = content.Split(':');
            IPAddress ip = IPAddress.Parse(remoteClient[0]);
            int port = int.Parse(remoteClient[1]);
            clientSocket.Shutdown(SocketShutdown.Both);
            peer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                peer.Connect(ip, port);
                Console.WriteLine("Successfully connect to the other client.");
            }
            catch
            {
                Console.WriteLine("Connection failed.");
                peer.Shutdown(SocketShutdown.Both);
                return;
            }

            connected.Set();
        }

        private void WaitForConnection()
        {
            string[] localClient = clientSocket.LocalEndPoint.ToString().Split(':');
            ip = IPAddress.Parse(localClient[0]);
            port = int.Parse(localClient[1]);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Bind(new IPEndPoint(ip, port));
            clientSocket.Listen(10);

            for (; ; )
            {
                peer = clientSocket.Accept();
                if (peer != null)
                    break;
            }
            
            Console.WriteLine("Successfully connect to the other client.");
            connected.Set();
        }

        public void StartClient()
        {
            port = 8885;
            started = false;
            connected = new ManualResetEvent(false);
            ip = IPAddress.Parse("172.21.228.123");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(ip, port);
                Console.WriteLine("Successfully connect to the server.");

            }
            catch
            {
                Console.WriteLine("Connection failed.");
                return;
            }
            Thread threadRead = new Thread(ListenServer);
            threadRead.Start();

            connected.WaitOne();
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Enter your message:");
                    string sendMessage = Console.ReadLine();
                    if (sendMessage.IndexOf("<EOF>") > -1)
                        break;
                    peer.Send(Encoding.ASCII.GetBytes("Heimdallr:" + sendMessage));

                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }

            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
