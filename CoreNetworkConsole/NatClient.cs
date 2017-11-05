using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CoreNetworkConsole
{
    public class NatClient
    {
        private static byte[] result = new byte[1024];
        private static Socket clientSocket;
        private static void ListenServer()
        {
            while (true)
            {
                result = new byte[1024];
                int contentLength = clientSocket.Receive(result);

                Console.WriteLine("{0}", Encoding.ASCII.GetString(result, 0, contentLength));
            }

        }
        public static void StartClient()
        {

            IPAddress ip = IPAddress.Parse("172.21.228.204");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(ip, 8885);
                Console.WriteLine("连接成功！");

            }
            catch
            {
                Console.WriteLine("连接失败！");
                return;
            }
            Thread threadRead = new Thread(ListenServer);
            threadRead.Start();


            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Enter your message:");
                    string sendMessage = Console.ReadLine();
                    if (sendMessage.IndexOf("<EOF>") > -1)
                        break;
                    clientSocket.Send(Encoding.ASCII.GetBytes("Heimdallr:" + sendMessage));

                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }

            }
            Console.WriteLine("发送完毕 按回车退出");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
