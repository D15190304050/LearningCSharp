using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CoreNetworkConsole
{
    public static class SyncListenr
    {
        public static void StartServer()
        {
            byte[] receiveBuffer = new byte[1024];

            IPAddress ip = new IPAddress(new byte[] { 172, 21, 228, 123 });
            IPEndPoint localEndPoint = new IPEndPoint(ip, 19887);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(2);

            try
            {
                Socket clientSocket = serverSocket.Accept();
                for (; ; )
                {
                    int receiveLength = clientSocket.Receive(receiveBuffer);
                    string message = Encoding.ASCII.GetString(receiveBuffer, 0, receiveLength);
                    if (message.IndexOf("<EOF>") > -1)
                        break;
                    message = DateTime.Now.ToString() + message;
                    clientSocket.Send(Encoding.ASCII.GetBytes(message));
                }
                clientSocket.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("stop here");
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }
    }
}
