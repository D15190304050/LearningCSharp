using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrent
{
    public static class TaskDemo
    {
        public static void CreateTasks()
        {
            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                    Task.CurrentId,
                    obj,
                    Thread.CurrentThread.ManagedThreadId);
            };

            // Create a task but do not start it.
            Task t1 = new Task(action, "Alpha");

            // Construct a task.
            Task t2 = Task.Factory.StartNew(action, "Beta");
            // Block the main thread to demonstrate that t2 is executing.
            t2.Wait();

            // Launch t1.
            t1.Start();
            Console.WriteLine("t1 has been launched. (Main Thread={0})", Thread.CurrentThread.ManagedThreadId);
            // Wait for the task to finish.
            t1.Wait();

            // Construct a started task using Task.Run().
            String taskData = "Delta";
            Task t3 = Task.Run(() => Console.WriteLine("Task={0}, obj={1}, Thread={2}", Task.CurrentId, taskData, Thread.CurrentThread.ManagedThreadId));
            // Wait for the task to finish.
            t3.Wait();

            // Construct an unstarted task.
            Task t4 = new Task(action, "Gamma");
            // Run it asynchronously.
            t4.RunSynchronously();
            // Although the task was run synchronously, it is a good practice to wait for it in the event exceptions were thrown by the task.
            t4.Wait();
        }

        public static void WaitForTask()
        {
            Task task = Task.Run(() => Thread.Sleep(2000));
            Console.WriteLine("task Status: {0}" + task.Status);
            try
            {
                task.Wait();
                Console.WriteLine("task Status: {0}" + task.Status);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in task.");
            }
        }

        public static void ConditionallyWait()
        {
            // Wait on a single task with a timeout specified.
            Task task = Task.Run(() => Thread.Sleep(2000));

            try
            {
                // Wait for 1 second.
                task.Wait(1000);
                bool completed = task.IsCanceled;
                Console.WriteLine("task completed: {0}, Status: {1}", completed, task.Status);

                if (!completed)
                    Console.WriteLine("Timeout before task completed.");
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in task.");
            }
        }

        public static void WaitAny()
        {
            Task[] tasks = new Task[3];
            Random random = new Random();
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = Task.Run(() => Thread.Sleep(random.Next(500, 3000)));

            try
            {
                int index = Task.WaitAny(tasks);
                Console.WriteLine("Task {0} completed first.\n", tasks[index].Id);
                Console.WriteLine("Status of all tasks:");
                foreach (Task t in tasks)
                    Console.WriteLine("    Task #{0}: {1}", t.Id, t.Status);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in task.");
            }
        }


    }
}
