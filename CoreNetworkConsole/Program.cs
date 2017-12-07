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
            //new Thread(NatHoleServer.StartServer).Start();

            //new Thread(new NatHoleClient().StartClient).Start();
            //new Thread(new NatHoleClient().StartClient).Start();

            NatHoleServer.StartServer();
            //new NatHoleClient().StartClient();

            //NatClient.StartClient();
            //NatServer.StartServer(8885);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}