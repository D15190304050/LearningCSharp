using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static void StartNew()
        {
            Task t = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 1000000; i++)
                    ;

                Console.WriteLine("Finished 1000000 loop iterations.");
            });

            t.Wait();
        }

        public static void WaitForTask()
        {
            Task t = Task.Run(() => Thread.Sleep(2000));
            Console.WriteLine("Task t Status: {0}", t.Status);

            try
            {
                t.Wait();
                Console.WriteLine("Task t Status: {0}", t.Status);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in Task t.");
            }
        }

        public static void TaskTimeOut()
        {
            // Wait on a single task with a timeout specified.
            Task t = Task.Run(() => Thread.Sleep(2000));

            try
            {
                // Wait for 1 second.
                t.Wait(1000);

                bool completed = t.IsCompleted;
                Console.WriteLine("Task t completed: {0}, Status: {1}", completed, t.Status);

                if (!completed)
                    Console.WriteLine("Timed out before Task t completed.");
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in Task t.");
            }
        }

        public static void WaitAnyDemo()
        {
            Task[] tasks = new Task[3];
            Random random = new Random();
            for (int i = 0; i < 3; i++)
                tasks[i] = Task.Run(() => Thread.Sleep(random.Next(500, 3000)));

            try
            {
                int indexOfFirstFinished = Task.WaitAny(tasks);

                Console.WriteLine("Task #{0} completed first.\n", tasks[indexOfFirstFinished].Id);
                Console.WriteLine("Status of all tasks:");
                foreach (Task t in tasks)
                    Console.WriteLine("    Task #{0}: {1}", t.Id, t.Status);
            }
            catch (AggregateException)
            {
                Console.WriteLine("An exception occurred.");
            }
        }

        public static void WaitAllDemo()
        {
            // Wait for all tasks to complete.
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
                tasks[i] = Task.Run(() => Thread.Sleep(2000));

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("One or more exceptions occurred.");
                foreach (Exception ex in e.Flatten().InnerExceptions)
                    Console.WriteLine("    {0}", ex.Message);
            }

            Console.WriteLine("Status of completed tasks:");
            foreach (Task t in tasks)
                Console.WriteLine("    Task #{0}: {1}", t.Id, t.Status);
        }

        public static void ExceptionsInTasks()
        {
            // Create a cancellation token and cancel it.
            CancellationTokenSource source1 = new CancellationTokenSource();
            CancellationToken token1 = source1.Token;
            source1.Cancel();

            // Create a cancellation token for later cancellation.
            CancellationTokenSource source2 = new CancellationTokenSource();
            CancellationToken token2 = source2.Token;

            // Create a series of tasks that will complete, be cancelled, timeout, or throw an exception.
            Task[] tasks = new Task[12];
            for (int i = 0; i < 12; i++)
            {
                switch (i % 4)
                {
                    // Task should run to completion.
                    case 0:
                        tasks[i] = Task.Run(() => Thread.Sleep(2000));
                        break;

                    // Task should set to canceled state.
                    case 1:
                        tasks[i] = Task.Run(() => Thread.Sleep(2000), token1);
                        break;

                    // Task should throw an exception.
                    case 2:
                        tasks[i] = Task.Run(() => { throw new NotSupportedException(); });
                        break;

                    // Task should examine cancellation token.
                    case 3:
                        tasks[i] = Task.Run(() =>
                        {
                            Thread.Sleep(2000);
                            if (token2.IsCancellationRequested)
                                token2.ThrowIfCancellationRequested();

                            Thread.Sleep(500);
                        }, token2);
                        break;
                }
            }

            Thread.Sleep(250);
            source2.Cancel();

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("One or more exceptions occurred:");
                foreach (Exception ex in e.Flatten().InnerExceptions)
                    Console.WriteLine("    {0}: {1}", ex.GetType().Name, ex.Message);
            }

            Console.WriteLine("Status of tasks:");
            foreach (Task t in tasks)
            {
                Console.WriteLine("    Task #{0}, {1}", t.Id, t.Status);
                if (t.Exception != null)
                {
                    foreach (Exception e in t.Exception.InnerExceptions)
                        Console.WriteLine("    {0}: {1}", e.GetType().Name, e.Message);
                }
            }
        }

        private static async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                StringBuilder contents = new StringBuilder();
                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                    contents.Append(text);
                }
                return contents.ToString();
            }
        }

        public static async Task ProcessReadAsync()
        {
            string filePath = "Resources/TrajectoriesCUHK_0-10000.txt";
            if (!File.Exists(filePath))
                Console.WriteLine("File not found: " + filePath);
            else
            {
                try
                {
                    string text = await ReadTextAsync(filePath);
                    Console.WriteLine(text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                Console.WriteLine("Finished...");
            }
        }

        public static async Task ProcessWriteAsync()
        {
            string filePath = "Resources/temp.txt";
            string text = "Hello World" + Environment.NewLine;
            
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            await WriteTextAsync(filePath, text);
        }

        public static async Task ProcessWriteMultiAsync()
        {
            string folder = "Resources/tempFolder/";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            List<Task> tasks = new List<Task>();
            List<FileStream> sourceStreams = new List<FileStream>();

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    string text = "In file " + i + Environment.NewLine;
                    string fileName = "theFile" + i.ToString("00") + ".txt";
                    string filePath = folder + fileName;

                    if (!File.Exists(filePath))
                        File.Create(filePath).Close();

                    byte[] encodedText = Encoding.UTF8.GetBytes(text);

                    FileStream sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);

                    Task writeTask = sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    sourceStreams.Add(sourceStream);
                    tasks.Add(writeTask);
                }

                await Task.WhenAll(tasks);
            }
            finally
            {
                foreach (FileStream sourceStream in sourceStreams)
                    sourceStream.Close();

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folder);
                Console.WriteLine(folderPath);
                Console.WriteLine("Done");
            }
        }
    }
}
