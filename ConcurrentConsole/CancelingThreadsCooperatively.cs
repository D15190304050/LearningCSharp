using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Concurrent
{
    public static class CancelingThreadsCooperatively
    {
        public static void Demo()
        {
            // The CancelingThreadsCooperatively class controls access to the token source.

        }
    }

    internal class ServerClass2
    {
        public static void StaticMethod(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;
            Console.WriteLine("ServerClass2.StaticMethod is running on another thread.");

            // Simulate work that can be canceled.
            while (!ct.IsCancellationRequested)
                Thread.SpinWait(50000);

            Console.WriteLine("The worker thread has been canceled.");
        }
    }
}