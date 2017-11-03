using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConsole
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //WebRequestGetExample.RequestGetExample();
            //WebRequestPostExample.RequestPostExample();
            //ClientGetAsyncDemo.Demo();
            //SyncSocketListener.StartListening();
            AsyncSocketListener.StartListening();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
