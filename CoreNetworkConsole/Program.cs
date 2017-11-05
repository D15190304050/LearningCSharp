using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CoreNetworkConsole
{
    public class Program
    {
        //public static int Main(string[] args)
        //{
        //    AsyncSocketListener.StartListening();

        //    // Keep the console window open in debug mode.
        //    Console.WriteLine("Press any key to continue...");
        //    Console.ReadKey();
        //    return 0;
        //}

        private static byte[] result = new byte[1024];

        static Socket serverSocket;
        private static string client;
        private static Socket clientSocket;
        //我这里存了两个Client，因为自己电脑开了两个Client，不会有多的
        //理论上应该开一个Socket[]来保存信息，最好用一个二元组将client的信息和连接绑定起来
        //这样就可以实现断开连接后下次登陆还是可以识别是这个Client
        private static Socket clientSocketA = null;
        private static Socket clientSocketB = null;

        static void Main(string[] args)
        {
            Program.SetPort(8885);
        }
        private static void SetPort(int port)
        {
            IPAddress ip = IPAddress.Parse("172.21.228.204");//set ip
            serverSocket = new Socket(AddressFamily.InterNetwork,
              SocketType.Stream, ProtocolType.Tcp);//initialize
            serverSocket.Bind(new IPEndPoint(ip, port));//bind
            serverSocket.Listen(10);
            //进入监听状态
            Console.WriteLine("监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //开启一个线程来监听客户端连接
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();

        }
        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private static void ListenClientConnect()
        {
            while (true)
            {
                //Client连接上后 得到这个连接
                clientSocket = serverSocket.Accept();

                //这里我因为只有两个Client，所以就简单写了
                if (clientSocketA == null)
                {
                    clientSocketA = clientSocket;
                }
                else if (clientSocketB == null)
                {
                    clientSocketB = clientSocket;
                }
                else
                {
                    //当其中一个断开了，又重新连接时，需要再次保存连接
                    if (clientSocketB.IsBound)
                    {
                        clientSocketA = clientSocketB;
                        clientSocketB = clientSocket;
                    }
                    else
                    {
                        clientSocketB = clientSocketA;
                        clientSocketA = clientSocket;
                    }

                }
                clientSocket.Send(Encoding.ASCII.GetBytes("say hello"));
                //开个线程接收Client信息
                Thread receivedThread = new Thread(ReceiveMessage);
                receivedThread.Start(clientSocket);

            }
        }

        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;

            while (true)
            {
                try
                {
                    int revceiveNumber = myClientSocket.Receive(result);
                    //Console.WriteLine("接受客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString()
                    //  , Encoding.ASCII.GetString(result, 0, revceiveNumber));
                    Console.WriteLine(Encoding.ASCII.GetString(result, 0, revceiveNumber));
                    if (myClientSocket == clientSocketA)
                    {
                        Console.WriteLine("receive from A");
                        if (clientSocketB != null && clientSocketB.IsBound)
                        {
                            Console.WriteLine("a IS BOUND");
                            clientSocketB.Send(result, 0, revceiveNumber, SocketFlags.None);
                        }
                        else
                        {
                            myClientSocket.Send(Encoding.ASCII.GetBytes("the people is not online! Send Failed!"));
                            Console.WriteLine("对方不在线上，发送失败！");
                        }
                    }
                    else
                    {
                        Console.WriteLine("receive from B");
                        if (clientSocketA != null && clientSocketA.IsBound)
                        {
                            Console.WriteLine("a IS BOUND");
                            clientSocketA.Send(result, 0, revceiveNumber, SocketFlags.None);
                        }
                        else
                        {
                            myClientSocket.Send(Encoding.ASCII.GetBytes("the people is not online! Send Failed!"));
                            Console.WriteLine("对方不在线上，发送失败！");
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;

                }
            }

        }

    }
}