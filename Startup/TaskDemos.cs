using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Startup
{
    public static class TaskDemos
    {
        public static void InstantiateTasks()
        {
            Action<object> action = (object o) =>
                Console.WriteLine($"Task={Task.CurrentId}, object={o}, Thread={Thread.CurrentThread.ManagedThreadId}");

            // Create a task but do not start it.
            Task t1 = new Task(action, "alpha");

            // Construct a started task.
            Task t2 = Task.Factory.StartNew(action, "beta");
            // Block the main thread to demonstrate that t2 is executing.
            t2.Wait();

            // Launch t1.
            t1.Start();
            Console.WriteLine($"t1 has been launched. (Main Thread={Thread.CurrentThread.ManagedThreadId})");
            // Wait for the task to finish.
            t1.Wait();

            // Construct a started task using Task.Run.
            Task t3 = Task.Run(() =>
                Console.WriteLine($"Task={Task.CurrentId}, object=delta, Thread={Thread.CurrentThread.ManagedThreadId}"));
            // Wait for the task to finish.
            t3.Wait();

            // Construct an unstarted task.
            Task t4 = new Task(action, "gamma");
            // Run it synchronously.
            t4.RunSynchronously();
            // Although the task was run synchronously, it is a good practice to wait for it in the event exceptions were thrown by the task.
            t4.Wait();
        }
    }
}
