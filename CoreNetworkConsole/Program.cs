using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using CoreNetworkConsole.DistributedSpanningTrees;

namespace CoreNetworkConsole
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //UnitTest.LanStandaloneTest();
            //UnitTest.LanMultiThreadTest();
            //NatClient.StartClient();
            //NatServer.StartServer(8885);

            //Thread serverThread = new Thread(SyncListenr.StartServer);
            //Thread clientThread = new Thread(SyncSender.StartClient);
            //serverThread.Start();
            //clientThread.Start();
            //SyncSender.StartClient();
            SyncListenr.StartServer();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}