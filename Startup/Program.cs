using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Startup
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //MySqlConnectionTest.ConnectionTest();
            //TaskDemos.InstantiateTasks();
            //TaskDemos.StartNew();
            //TaskDemos.WaitForTask();
            //TaskDemos.TaskTimeOut();
            //TaskDemos.WaitAnyDemo();
            //TaskDemos.WaitAllDemo();
            //TaskDemos.ExceptionsInTasks();
            //_ = TaskDemos.ProcessReadAsync();
            //_ = TaskDemos.ProcessWriteAsync();
            _ = TaskDemos.ProcessWriteMultiAsync();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}