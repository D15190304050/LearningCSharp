using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CoreNetworkConsole
{
    public static class SyncSender
    {
        public static void StartClient()
        {
            byte[] receiveBuffer = new byte[1024];

            IPAddress ip = new IPAddress(new byte[] { 172, 21, 228, 123 });
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(ip, 19887);

            string message;
            for (int i = 0; i < 5; i++)
            {
                //Console.WriteLine("Please enter what you want to send to server (end with enter): ");
                //message = Console.ReadLine();
                message = i.ToString();
                clientSocket.Send(Encoding.ASCII.GetBytes(message));
                int receiveLength = clientSocket.Receive(receiveBuffer);
                message = Encoding.ASCII.GetString(receiveBuffer, 0, receiveLength);
                Console.WriteLine("Message received from server: " + message);
            }
            clientSocket.Send(Encoding.ASCII.GetBytes("<EOF>"));
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
