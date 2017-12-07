using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoreNetworkConsole
{
    public class NatHoleServer
    {
        private static byte[] messageToForward = new byte[1024];

        private static Socket serverSocket;
        //private static string client;
        private static Socket clientSocket;
        //我这里存了两个Client，因为自己电脑开了两个Client，不会有多的
        //理论上应该开一个Socket[]来保存信息，最好用一个二元组将client的信息和连接绑定起来
        //这样就可以实现断开连接后下次登陆还是可以识别是这个Client
        private static Socket clientSocketA = null;
        private static Socket clientSocketB = null;

        public static void StartServer() => SetPort(8885);

        private static void SetPort(int port)
        {
            IPAddress ip = IPAddress.Parse("172.21.228.123");//set ip
            serverSocket = new Socket(AddressFamily.InterNetwork,
              SocketType.Stream, ProtocolType.Tcp);//initialize
            serverSocket.Bind(new IPEndPoint(ip, port));//bind
            serverSocket.Listen(10);
            //进入监听状态
            Console.WriteLine("监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //开启一个线程来监听客户端连接
            Thread listenThread = new Thread(ListenClientConnect);
            listenThread.Start();

            listenThread.Join();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private static void ListenClientConnect()
        {
            while (true)
            {
                clientSocket = serverSocket.Accept();

                if (clientSocketA == null)
                {
                    clientSocketA = clientSocket;
                    clientSocket.Send(Encoding.ASCII.GetBytes("say hello"));
                }
                else if (clientSocketB == null)
                {
                    clientSocketB = clientSocket;
                    clientSocket.Send(Encoding.ASCII.GetBytes("say hello"));
                }

                if ((clientSocketA != null) && (clientSocketB != null))
                {
                    string connectionInfo = clientSocketB.RemoteEndPoint.ToString();
                    clientSocketB.Send(Encoding.ASCII.GetBytes("Listen"));
                    clientSocketA.Send(Encoding.ASCII.GetBytes(connectionInfo));
                    break;
                }
            }
        }
    }
}
